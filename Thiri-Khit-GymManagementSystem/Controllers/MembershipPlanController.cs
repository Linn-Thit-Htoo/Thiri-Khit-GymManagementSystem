using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Thiri_Khit_GymManagementSystem.Models;

namespace Thiri_Khit_GymManagementSystem.Controllers
{
    public class MembershipPlanController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public MembershipPlanController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [ActionName("MembershipPlanManagement")]
        public async Task<IActionResult> Index()
        {
            try
            {
                List<MembershipPlanDataModel> lst = await _appDbContext.MembershipPlan
                    .OrderByDescending(x => x.MembershipPlanId)
                    .ToListAsync();

                return View(lst);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IActionResult CreateMembershipPlan()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(MembershipPlanDataModel dataModel)
        {
            try
            {
                bool isDuplicate = await _appDbContext.MembershipPlan
                     .AnyAsync(x => x.PlanName == dataModel.PlanName.ToLower());
                if (isDuplicate)
                {
                    TempData["error"] = "Plan name already exists!";
                    return RedirectToAction("MembershipPlanManagement");
                }

                dataModel.PlanName = dataModel.PlanName.ToLower();
                await _appDbContext.MembershipPlan.AddAsync(dataModel);
                int result = await _appDbContext.SaveChangesAsync();

                if (result > 0)
                {
                    TempData["success"] = "Creating Successful!";
                }
                else
                {
                    TempData["error"] = "Creating Fail!";
                }
                return RedirectToAction("MembershipPlanManagement");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IActionResult> EditMembershipPlan(long id)
        {
            try
            {
                MembershipPlanDataModel? membershipPlan = await _appDbContext.MembershipPlan
                    .FirstOrDefaultAsync(x => x.MembershipPlanId == id);

                return View(membershipPlan);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(MembershipPlanDataModel dataModel)
        {
            try
            {
                bool isDuplicate = await _appDbContext.MembershipPlan
                    .AnyAsync(x => x.PlanName == dataModel.PlanName && x.MembershipPlanId != dataModel.MembershipPlanId);
                if (isDuplicate)
                {
                    TempData["error"] = "Plan name already exists!";
                    return RedirectToAction("MembershipPlanManagement");
                }

                MembershipPlanDataModel? membershipPlan = await _appDbContext.MembershipPlan
                    .FirstOrDefaultAsync(x => x.MembershipPlanId == dataModel.MembershipPlanId);

                if (membershipPlan is null)
                {
                    TempData["error"] = "Updating Fail!";
                    return RedirectToAction("MembershipPlanManagement");
                }

                membershipPlan.PlanName = dataModel.PlanName;
                membershipPlan.Price = dataModel.Price;
                int result = await _appDbContext.SaveChangesAsync();

                if (result > 0)
                {
                    TempData["success"] = "Updating Successful!";
                }
                else
                {
                    TempData["error"] = "Updating Fail!";
                }

                return RedirectToAction("MembershipPlanManagement");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IActionResult> Delete(long id)
        {
            MembershipPlanDataModel? membershipPlan = await _appDbContext.MembershipPlan
                .FirstOrDefaultAsync(x => x.MembershipPlanId == id);
            if (membershipPlan is null)
            {
                TempData["error"] = "Deleting Fail!";
                return RedirectToAction("MembershipPlanManagement");
            }

            _appDbContext.MembershipPlan.Remove(membershipPlan);
            int result = await _appDbContext.SaveChangesAsync();

            if (result > 0)
            {
                TempData["success"] = "Deleting Successful!";
            }
            else
            {
                TempData["error"] = "Deleting Fail!";
            }
            return RedirectToAction("MembershipPlanManagement");
        }
    }
}
