using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MicroStack.Core.Entitties;
using MicroStack.UI.ViewModel;

namespace MicroStack.UI.Controllers
{
    public class HomeController : Controller
    {
        public UserManager<AppUser> _userManager { get; }
        public SignInManager<AppUser> _signInManager { get; }

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string? returnURL)
        {
            returnURL = returnURL ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());
                        //return RedirectToAction("Index");
                        return LocalRedirect(returnURL);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email address is no valid or password");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email address is no valid or password");
                }
            }
            return View(loginViewModel);
        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(AppUserViewModel appUserViewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser();
                user.FirstName = appUserViewModel.FirstName;
                user.Email = appUserViewModel.Email;
                user.LastName = appUserViewModel.LastName;
                user.PhoneNumber = appUserViewModel.PhoneNumber;
                user.UserName = appUserViewModel.UserName;
                if (appUserViewModel.UserSelectTypeId == 1)
                {
                    user.IsBuyer = true;
                    user.IsSeller = false;
                }
                else
                {
                    user.IsSeller = true;
                    user.IsBuyer = false;
                }

                var result = await _userManager.CreateAsync(user, appUserViewModel.Password);

                if (result.Succeeded)
                    return RedirectToAction("Login");
                else
                {
                    foreach (IdentityError item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(appUserViewModel);
        }

        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
