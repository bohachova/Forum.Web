using Forum.Web.Interfaces;
using Forum.Web.Models.User;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Forum.Web.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly IForumAPI forumAPI;
        private readonly IHttpContextAccessor accessor;
        public AuthorizationController(IForumAPI forumAPI, IHttpContextAccessor accessor)
        {
            this.forumAPI= forumAPI;
            this.accessor = accessor;
        }
        [HttpGet]
        public IActionResult EnterLogin()
        {
            return View("AuthorizationEnterLogin");
        }

        [HttpPost]
        public async Task<IActionResult> CheckIfPasswordSetRequired(string email)
        {
            var required = await forumAPI.CheckIfPasswordSetRequired(email);
            if (required)
            {
                return RedirectToAction("SetPassword", "Authorization", new { email });
            }
            else
            {
                return RedirectToAction("EnterPassword", "Authorization", new { email });
            }
        }

        [HttpGet]
        public IActionResult EnterPassword(string email)
        {
            var model = new UserAuthorizationModel { Email = email };
            return View("AuthorizationEnterPassword", model);
        }
        [HttpPost]
        public async Task<IActionResult> EnterPassword(UserAuthorizationModel model)
        {
            var result = await forumAPI.Authorize(model);
            if (result.IsSuccess)
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(result.JWTToken);
                List<Claim> claims = new List<Claim>() { 
                    new Claim(ClaimTypes.Name, jwt.Claims.First(x => x.Type == ClaimTypes.Name).Value), 
                    new Claim(ClaimTypes.Role, jwt.Claims.First(x => x.Type == ClaimTypes.Role).Value),
                    new Claim("JWTToken", result.JWTToken),
                    new Claim("UserId", jwt.Claims.First(x=> x.Type == "UserId").Value),
                    new Claim("MutedUser", jwt.Claims.FirstOrDefault(x => x.Type == "MutedUser").Value)
                };
                ClaimsIdentity claimsId = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                DateTimeOffset expirationTime = DateTimeOffset.Now.AddDays(1);
                await accessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsId), new AuthenticationProperties { ExpiresUtc = expirationTime });
                return RedirectToAction("ForumMainPage", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = result.Message;
                return View("AuthorizationEnterPassword", model);
            }
        }
        [HttpGet]
        public IActionResult SetPassword (string email)
        {
            var model = new UserAuthorizationModel { Email = email };
            return View("SetAdminPassword", model);
        }
        [HttpPost]
        public async Task<IActionResult> SetPassword(UserAuthorizationModel model)
        {
            var result = await forumAPI.SetAdminPassword(model);
            if (result.IsSuccess)
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(result.JWTToken);
                List<Claim> claims = new List<Claim>() { 
                    new Claim(ClaimTypes.Name, jwt.Claims.First(x => x.Type == ClaimTypes.Name).Value), 
                    new Claim(ClaimTypes.Role, jwt.Claims.First(x => x.Type == ClaimTypes.Role).Value),
                    new Claim("JWTToken", result.JWTToken),
                    new Claim("UserId", jwt.Claims.First(x=> x.Type == "UserId").Value)
                };
                ClaimsIdentity claimsId = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                DateTimeOffset expirationTime = DateTimeOffset.Now.AddDays(1);
                await accessor.HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsId), new AuthenticationProperties { ExpiresUtc = expirationTime });
                return RedirectToAction("ForumMainPage", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = result.Message;
                return View("SetAdminPassword", model);
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await accessor.HttpContext!.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
