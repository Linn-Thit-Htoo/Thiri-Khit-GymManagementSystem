using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Thiri_Khit_GymManagementSystem.Models;

namespace Thiri_Khit_GymManagementSystem.Controllers;

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
            HttpContext.Session.SetString("id", user.UserId.ToString());
            HttpContext.Session.SetString("name", user.UserName);

            return RedirectToAction("UserManagement", "User");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [ActionName("UserManagement")]
    public async Task<IActionResult> GoToUserManagement()
    {
        try
        {
            if (HttpContext.Session.GetString("id") is null)
            {
                return RedirectToAction("LoginPage");
            }

            List<UserDataModel> users = await _appDbContext.Users
                .OrderByDescending(x => x.UserId)
                .ToListAsync();
            return View(users);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [ActionName("CreateUser")]
    public IActionResult GoToCreateUserPage()
    {
        if (HttpContext.Session.GetString("id") is null)
        {
            return RedirectToAction("LoginPage");
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Save(UserDataModel userDataModel)
    {
        try
        {
            UserDataModel? user = await _appDbContext.Users
                .Where(x => x.Email == userDataModel.Email)
                .FirstOrDefaultAsync();

            if (user is not null)
            {
                TempData["error"] = "User with this email already exists!";
                return RedirectToAction("UserManagement");
            }

            userDataModel.UserRole = "member";
            await _appDbContext.Users.AddAsync(userDataModel);
            int result = await _appDbContext.SaveChangesAsync();

            if (result > 0)
            {
                TempData["success"] = "Creating Successful!";
            }
            else
            {
                TempData["error"] = "Creating Fail!";
            }
            return RedirectToAction("UserManagement");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<IActionResult> EditUser(long id)
    {
        try
        {
            if (HttpContext.Session.GetString("id") is null)
            {
                return RedirectToAction("LoginPage");
            }

            UserDataModel? user = await _appDbContext.Users
                .Where(x => x.UserId == id)
                .FirstOrDefaultAsync();

            return View(user);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Update(UserDataModel userDataModel)
    {
        try
        {

            bool isDuplicate = await _appDbContext
                .Users
                .Where(x => x.Email == userDataModel.Email && x.UserId != userDataModel.UserId)
                .AnyAsync();
            if (isDuplicate)
            {
                TempData["error"] = "User with this email already exists!";
                return RedirectToAction("UserManagement");
            }

            UserDataModel? user = await _appDbContext.Users
                .Where(x => x.UserId == userDataModel.UserId)
                .FirstOrDefaultAsync();

            if (user is not null)
            {
                user.UserName = userDataModel.UserName;
                user.Email = userDataModel.Email;
                int result = await _appDbContext.SaveChangesAsync();

                if (result > 0)
                {
                    TempData["success"] = "Updating Successful!";
                }
                else
                {
                    TempData["error"] = "Updating Fail!";
                }
                return RedirectToAction("UserManagement");
            }
            TempData["error"] = "User not found.";
            return RedirectToAction("UserManagement");
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
            if (HttpContext.Session.GetString("id") is null)
            {
                return RedirectToAction("LoginPage");
            }

            UserDataModel? user = await _appDbContext
                .Users
                .FirstOrDefaultAsync(x => x.UserId == id);
            if (user is null)
            {
                TempData["Title"] = "User not found!";
                return RedirectToAction("UserManagement");
            }

            _appDbContext.Users.Remove(user);
            int result = await _appDbContext.SaveChangesAsync();

            if (result > 0)
            {
                TempData["success"] = "Deleting Successful!";
            }
            else
            {
                TempData["error"] = "Deleting Fail!";
            }
            return RedirectToAction("UserManagement");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public IActionResult ProfileSetting()
    {
        try
        {
            string? id = HttpContext.Session.GetString("id");
            string? name = HttpContext.Session.GetString("name");
            TempData["id"] = id;
            TempData["name"] = name;
            return View();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProfile(UpdateProfileRequestModel requestModel)
    {
        try
        {
            UserDataModel? user = await _appDbContext.Users
                .FirstOrDefaultAsync(x => x.UserId == requestModel.UserId);
            if (user is null)
            {
                TempData["error"] = "Updating Fail!";
                return RedirectToAction("UserManagement");
            }

            user.UserName = requestModel.UserName;
            int result = await _appDbContext.SaveChangesAsync();

            if (result > 0)
            {
                TempData["successMsg"] = "Updating Successful!";
            }
            else
            {
                TempData["errorMsg"] = "Updating Fail!";
            }
            return RedirectToAction("LoginPage");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    [ActionName("ChangePasswordPage")]
    public IActionResult GoToChangePasswordPage()
    {
        string? id = HttpContext.Session.GetString("id");
        TempData["id"] = id;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequestModel requestModel)
    {
        try
        {
            if (requestModel.NewPassword != requestModel.ConfirmPassword)
            {
                TempData["error"] = "Passwords must be the same!";
                return RedirectToAction("ChangePasswordPage");
            }

            UserDataModel? user = await _appDbContext.Users
                .FirstOrDefaultAsync(x => x.UserId == requestModel.UserId && x.Password == requestModel.OldPassword);
            if (user is null)
            {
                TempData["error"] = "Old password incorrect!";
                return RedirectToAction("ChangePasswordPage");
            }

            user.Password = requestModel.ConfirmPassword;
            _appDbContext.Entry(user).State = EntityState.Modified;
            int result = await _appDbContext.SaveChangesAsync();

            if (result > 0)
            {
                TempData["successMsg"] = "Updating Successful!";
            }
            else
            {
                TempData["error"] = "Updating Fail!";
                return RedirectToAction("ChangePasswordPage");
            }
            return RedirectToAction("LoginPage");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}