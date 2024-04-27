using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using Thiri_Khit_GymManagementSystem.Models;

namespace Thiri_Khit_GymManagementSystem.Controllers
{
    public class CheckInCheckOutController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public CheckInCheckOutController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult CheckInCheckOutManagement()
        {
            try
            {
                string query = @"SELECT Users.UserName, Users.UserRole, Check_In_Check_Out.CICOId, Check_In_Check_Out.CheckInDateTime,
Check_In_Check_Out.CheckOutDateTime
FROM Check_In_Check_Out
INNER JOIN Users ON Users.UserId = Check_In_Check_Out.UserId";
                SqlConnection conn = new(_configuration.GetConnectionString("DbConnection"));
                conn.Open();
                SqlCommand cmd = new(query, conn);
                SqlDataAdapter adapter = new(cmd);
                DataTable dt = new();
                adapter.Fill(dt);
                conn.Close();

                string jsonStr = JsonConvert.SerializeObject(dt);
                List<CheckInCheckOutResponseModel> lst = JsonConvert.DeserializeObject<List<CheckInCheckOutResponseModel>>(jsonStr)!;

                return View(lst);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IActionResult> CreateCheckInCheckOut()
        {
            try
            {
                List<UserDataModel> users = await _context.Users.Where(x => x.UserRole != "admin").ToListAsync();

                return View(users);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save(CheckInCheckOutDataModel dataModel)
        {
            try
            {
                await _context.Check_In_Check_Out.AddAsync(dataModel);
                int result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    TempData["success"] = "Creating Successful!";
                }
                else
                {
                    TempData["error"] = "Creating Fail!";
                }
                return RedirectToAction("CheckInCheckOutManagement");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(long id)
        {
            try
            {
                bool isDuplicate = await _context.Check_In_Check_Out
                    .AnyAsync(x => x.CICOId == id && !string.IsNullOrEmpty(x.CheckOutDateTime));
                if (isDuplicate)
                {
                    TempData["error"] = "Already checkout!";
                    return RedirectToAction("CheckInCheckOutManagement");
                }

                CheckInCheckOutDataModel? item = await _context.Check_In_Check_Out
                    .FirstOrDefaultAsync(x => x.CICOId == id);
                if (item is null)
                {
                    TempData["error"] = "No data found.";
                    return RedirectToAction("CheckInCheckOutManagement");
                }

                item.CheckOutDateTime = Convert.ToString(DateTime.Now);
                int result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    TempData["success"] = "Successful!";
                }
                else
                {
                    TempData["error"] = "Fail!";
                }

                return RedirectToAction("CheckInCheckOutManagement");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IActionResult> EditCheckInCheckOut(long id)
        {
            try
            {
                CheckInCheckOutDataModel? item = await _context.Check_In_Check_Out
                    .FirstOrDefaultAsync(x => x.CICOId == id);
                if (item is null)
                {
                    TempData["error"] = "No data found.";
                    return RedirectToAction("CheckInCheckOutManagement");
                }

                List<UserDataModel> users = await _context.Users.Where(x => x.UserRole != "admin").ToListAsync();

                EditCheckInCheckOutResponseModel responseModel = new()
                {
                    Users = users,
                    CheckInCheckOutDataModel = item
                };

                return View(responseModel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCheckInCheckOut(UpdateCheckInCheckOutRequestModel requestModel)
        {
            try
            {
                CheckInCheckOutDataModel? item = await _context.Check_In_Check_Out
                    .FirstOrDefaultAsync(x => x.CICOId == requestModel.CICOId);
                if (item is null)
                {
                    TempData["error"] = "No data found.";
                    return RedirectToAction("CheckInCheckOutManagement");
                }

                item.UserId = requestModel.UserId;
                item.CheckInDateTime = requestModel.CheckInDateTime;
                int result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    TempData["success"] = "Updating Successful!";
                }
                else
                {
                    TempData["error"] = "Updating Fail!";
                }

                return RedirectToAction("CheckInCheckOutManagement");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                CheckInCheckOutDataModel? item = await _context.Check_In_Check_Out.FirstOrDefaultAsync(x => x.CICOId == id);
                if (item is null)
                {
                    TempData["error"] = "No data found.";
                    return RedirectToAction("CheckInCheckOutManagement");
                }

                _context.Check_In_Check_Out.Remove(item);
                int result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    TempData["success"] = "Deleting Successful!";
                }
                else
                {
                    TempData["error"] = "Deleting Fail!";
                }
                return RedirectToAction("CheckInCheckOutManagement");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
