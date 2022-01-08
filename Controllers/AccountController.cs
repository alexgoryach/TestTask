using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestTask.Models;
using TestTask.ViewModels;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
 
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /*[HttpGet("Register")]
        public IActionResult Register()
        {
            return Ok();
        }*/

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            User user = new User { Email = model.Email, UserName = model.Email};
            // Adding user
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Preparing cookies
                await _signInManager.SignInAsync(user, false);

            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Ok();
        }
        
        /*[HttpGet("Login")]
        public IActionResult Login(string returnUrl = null)
        {
            /*return View(new LoginViewModel { ReturnUrl = returnUrl });#1#
        }*/
 
        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = 
                await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            /*if (result.Succeeded)
            {
                // Checking the link belongs to API
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            }*/
            return Ok();
        }
 
        [HttpPost("Logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Delete authentication cookies
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}