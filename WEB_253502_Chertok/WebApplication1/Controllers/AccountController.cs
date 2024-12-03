using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_253502_Chertok.Models;
using WEB_253502_Chertok.Services.Authentication;

namespace WEB_253502_Chertok.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterUserViewModel());
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel user, [FromServices] IAuthService authService)
        {
            if (ModelState.IsValid)
            {
                if (user == null)
                {
                    return BadRequest();
                }

                var result = await authService.RegisterUserAsync(user.Email, user.Password, user.Avatar);

                if (result.Result)
                {
                    return Redirect(Url.Action("Index", "Home"));
                }
                else
                {
                    return BadRequest(result.ErrorMessage);
                }
            }
            return View(user);
        }

        public async Task Login()
        {
            var redirectUri = Url.Action("Index", "Home");
            Console.WriteLine($"RedirectUri: {redirectUri}");
            await HttpContext.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = Url.Action("Index", "Home", null, Request.Scheme)
            });
        }

        [HttpPost]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = Url.Action("Index", "Home", null, Request.Scheme)
            });
        }
    }
}
