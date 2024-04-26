using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using Thiri_Khit_GymManagementSystem.Models;

namespace Thiri_Khit_GymManagementSystem.Controllers
{
    public class MembershipController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _appDbContext;

        public MembershipController(IConfiguration configuration, AppDbContext appDbContext)
        {
            _configuration = configuration;
            _appDbContext = appDbContext;
        }

        [ActionName("MembershipManagement")]
        public IActionResult Index()
        {
            try
            {
                string query = @"SELECT Users.UserName,
Membership_Plan.PlanName, Membership_Plan.Price,
Membership.MembershipId, Membership.StartDate, Membership.EndDate
FROM Membership
INNER JOIN Membership_Plan ON Membership.MembershipPlanId = Membership_Plan.MembershipPlanId
INNER JOIN Users ON Users.UserId = Membership.UserId
ORDER BY Membership.MembershipId DESC";
                SqlConnection conn = new(_configuration.GetConnectionString("DbConnection"));
                conn.Open();
                SqlCommand cmd = new(query, conn);
                SqlDataAdapter adapter = new(cmd);
                DataTable dt = new();
                adapter.Fill(dt);
                conn.Close();

                string jsonStr = JsonConvert.SerializeObject(dt);
                List<MembershipResponseModel> lst = JsonConvert.DeserializeObject<List<MembershipResponseModel>>(jsonStr)!;

                return View(lst);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IActionResult> CreateMembership()
        {
            try
            {
                List<UserDataModel> users = await _appDbContext.Users
                    .Where(x => x.UserRole != "admin")
                    .ToListAsync();
                List<MembershipPlanDataModel> plans = await _appDbContext.MembershipPlan.ToListAsync();

                CreateMembershipResponseModel responseModel = new()
                {
                    Users = users,
                    Plans = plans
                };

                return View(responseModel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save(MembershipDataModel dataModel)
        {
            try
            {
                await _appDbContext.Membership.AddAsync(dataModel);
                int result = await _appDbContext.SaveChangesAsync();

                if (result > 0)
                {
                    TempData["success"] = "Creating Successful!";
                }
                else
                {
                    TempData["error"] = "Creating Fail!";
                }
                return RedirectToAction("MembershipManagement");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IActionResult> EditMembership(long id)
        {
            try
            {
                MembershipDataModel? membership = await _appDbContext.Membership
                    .FirstOrDefaultAsync(x => x.MembershipId == id);

                List<MembershipPlanDataModel> plans = await _appDbContext.MembershipPlan
                    .ToListAsync();

                List<UserDataModel> users = await _appDbContext.Users
                    .Where(x => x.UserRole != "admin")
                    .ToListAsync();

                EditMembershipResponseModel responseModel = new()
                {
                    Membership = membership,
                    Users = users,
                    Plans = plans
                };

                return View(responseModel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(MembershipDataModel dataModel)
        {
            try
            {
                MembershipDataModel? membership = await _appDbContext.Membership
                    .FirstOrDefaultAsync(x => x.MembershipId == dataModel.MembershipId);
                if (membership is null)
                {
                    TempData["error"] = "Updating Fail!";
                    return RedirectToAction("MembershipManagement");
                }

                membership.MembershipPlanId = dataModel.MembershipPlanId;
                membership.UserId = dataModel.UserId;
                membership.StartDate = dataModel.StartDate;
                membership.EndDate = dataModel.EndDate;
                int result = await _appDbContext.SaveChangesAsync();

                if (result > 0)
                {
                    TempData["success"] = "Updating Successful!";
                }
                else
                {
                    TempData["error"] = "Updating Fail!";
                }

                return RedirectToAction("MembershipManagement");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IActionResult> DeleteMembership(long id)
        {
            try
            {
                MembershipDataModel? membership = await _appDbContext.Membership
                    .FirstOrDefaultAsync(x => x.MembershipId == id);
                if (membership is null)
                {
                    TempData["error"] = "Deleting Fail!";
                    return RedirectToAction("MembershipManagement");
                }

                _appDbContext.Membership.Remove(membership);
                int result = await _appDbContext.SaveChangesAsync();

                if (result > 0)
                {
                    TempData["success"] = "Deleting Successful!";
                }
                else
                {
                    TempData["error"] = "Deleting Fail!";
                }
                return RedirectToAction("MembershipManagement");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
