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
            if(!string.IsNullOrEmpty(userViewModel.Password) &&
                !string.IsNullOrEmpty(userViewModel.ConfirmPassword) &&
                userViewModel.Password == userViewModel.ConfirmPassword) {
                userViewModel.Password = EncryptFunctions.Sha256Converter(userViewModel.ConfirmPassword);

                if (ModelState.IsValid)
                {
                    _context.Add(new User { UserName = userViewModel.UserName, Password = userViewModel.Password });
                    await _context.SaveChangesAsync();
                    ViewData["Messaje"] = "User created properly";
                    return RedirectToAction("Login", "Access");
                }

                ViewData["Messaje"] = "You need to fill all the fields";
            }
            else
            {
                ViewData["Messaje"] = "Passwords are not equal";
            }
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(UserViewModel userViewModel)
        {
            if (!string.IsNullOrEmpty(userViewModel.Password))
            {
                var cryptedPassword = EncryptFunctions.Sha256Converter(userViewModel.Password);
                var user = await _context.Users.FirstAsync(x => x.Password == cryptedPassword).ConfigureAwait(false);

                if (user != null && user.UserId != 0)
                {
                    HttpContext.Session.SetString(SessionVariables.SessionEnum.SessionKeyUserName.ToString(), user.UserName);
                    HttpContext.Session.SetString(SessionVariables.SessionEnum.SessionKeyId.ToString(), Guid.NewGuid().ToString());

                    return RedirectToAction("Index", "Home");
                }
            }

            ViewData["Messaje"] = "User or password not correct";

            return View();
        }
    }
}
