using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Thiri_Khit_GymManagementSystem.Models;

namespace Thiri_Khit_GymManagementSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public UserController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult LoginPage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserDataModel dataModel)
        {
            try
            {
                UserDataModel? user = await _appDbContext.Users
                    .Where(x => x.Email == dataModel.Email && x.Password == dataModel.Password && x.UserRole == "admin")
                    .FirstOrDefaultAsync();
                if (user is null)
                {
                    TempData["error"] = "Login Fail.";
                    return RedirectToAction("LoginPage");
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
