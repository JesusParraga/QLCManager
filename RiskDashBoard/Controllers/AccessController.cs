using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiskDashBoard.Context;
using RiskDashBoard.Models;
using RiskDashBoard.Models.ViewModels;
using RiskDashBoard.Resources;

namespace RiskDashBoard.Controllers
{
    public class AccessController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccessController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel userViewModel)
        {
            if (userViewModel.ValidateObjectProperties()) {
                if (userViewModel.Password == userViewModel.ConfirmPassword)
                {
                    userViewModel.Password = EncryptFunctions.Sha256Converter(userViewModel.ConfirmPassword);

                    if (await _context.Users.CountAsync(x => x.Email == userViewModel.Email).ConfigureAwait(false) == 0)
                    {
                        _context.Add(new User { UserName = userViewModel.UserName, Password = userViewModel.Password, Email = userViewModel.Email });
                        await _context.SaveChangesAsync().ConfigureAwait(false);
                        return RedirectToAction("Login", "Access");
                    }
                    ViewData["message"] = "This email is registerd in the system";           
                }
                else{
                    ViewData["message"] = "Passwords are not equal";
                }
            }
            else{
				ViewData["message"] = "You need to fill all the fields";
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(UserViewModel userViewModel)
        {
            if (!string.IsNullOrEmpty(userViewModel.Password))
            {
                var cryptedPassword = EncryptFunctions.Sha256Converter(userViewModel.Password);
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Password == cryptedPassword).ConfigureAwait(false);

                if (user != null && user.UserId != 0 && !string.IsNullOrEmpty(user.UserName))
                {
                    HttpContext.Session.SetString(SessionVariables.SessionEnum.SessionKeyUserName.ToString(), user.UserId.ToString());
                    HttpContext.Session.SetString(SessionVariables.SessionEnum.SessionKeyId.ToString(), Guid.NewGuid().ToString());

                    return RedirectToAction("Index", "Projects");
                }
            }

            ViewData["message"] = "User or password not correct";

            return View();
        }

        public ActionResult LogOut()
        {
            HttpContext.Session.SetString(SessionVariables.SessionEnum.SessionKeyUserName.ToString(), string.Empty);
            HttpContext.Session.SetString(SessionVariables.SessionEnum.SessionKeyId.ToString(), string.Empty);

            return RedirectToAction("Login", "Access");
        }
    }
}
