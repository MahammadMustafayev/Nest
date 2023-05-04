using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NestTest.Models;
using NestTest.ViewModels.Auth;

namespace NestTest.Controllers
{
    public class AuthController : Controller
    {
        private UserManager<AppUser> _usermanager { get;  }
        private SignInManager<AppUser> _signmanager { get;  }
        private RoleManager<IdentityRole>  _roleManager { get; }
        public AuthController(UserManager<AppUser> usermanager, SignInManager<AppUser> signmanager, RoleManager<IdentityRole> roleManager)
        {
            _usermanager = usermanager;
            _signmanager = signmanager;
            _roleManager = roleManager;
        }
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM, string returnUrl)
        {
            AppUser existUser= await _usermanager.FindByNameAsync(loginVM.UserNameOrEmail); 
            //if (loginVM.UserNameOrEmail.Contains("@"))
            //{
            //    user = await _usermanager.FindByEmailAsync(loginVM.UserNameOrEmail);
            //}
            //else
            //{
            //    user = await _usermanager.FindByNameAsync(loginVM.UserNameOrEmail); 
            //}
            if (existUser == null)
            {
                ModelState.AddModelError("", "UserName or Password incorrect");
                return View(loginVM);
            }
            var result = await _signmanager.PasswordSignInAsync(existUser, loginVM.Password, loginVM.RememberMe, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "You made too many attemps. Wait untill -> " + existUser.LockoutEnd?.AddHours(4).DateTime.ToString("MM/dd/yyyy HH:mm:ss"));
                    return View();
                }
                ModelState.AddModelError("", "Username or password is wrong");
                return View();
            }   
            if (returnUrl is null) return RedirectToAction("Index", "Home");
            return Redirect(returnUrl);
           
        }
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register( RegisterVM register )
        {
           
            AppUser appUser = new AppUser 
            {
                Name=register.FirstName,
                Surname=register.LastName,
                Email=register.Email,
                UserName=register.UserName,
            };
            IdentityResult result = await _usermanager.CreateAsync(appUser,register.Password);
            //if (!ModelState.IsValid) return View();

            //if (!result.Succeeded)
            //{
            //    foreach (var error in result.Errors)
            //    {
            //        ModelState.AddModelError("", error.ToString());
            //    }
            //    return View();
            //}
            await _signmanager.SignInAsync(appUser,true);
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> SignOut()
        {
            await _signmanager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
