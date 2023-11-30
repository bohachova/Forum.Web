using Microsoft.AspNetCore.Mvc;
using Forum.Web.Interfaces;
using Forum.Web.Models.User;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Forum.Web.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IForumAPI forumAPI;
        private readonly IHttpContextAccessor accessor;
        public RegistrationController(IForumAPI forumAPI, IHttpContextAccessor accessor)
        {
            this.forumAPI = forumAPI;
            this.accessor = accessor;
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View("RegistrationForm");
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(UserRegistrationModel model)
        {
            if(ModelState.IsValid)
            {
                var result = await forumAPI.CreateUser(model);
                if (result.IsSuccess)
                {
                    var jwt = new JwtSecurityTokenHandler().ReadJwtToken(result.JWTToken);
                    List<Claim> claims = new List<Claim>() {
                    new Claim(ClaimTypes.Name, jwt.Claims.First(x=>x.Type == ClaimTypes.Name).Value),
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
                    return View("RegistrationForm", model);
                }
            }
            ViewBag.ErrorMessage = "Invalid data entered";
            return View("RegistrationForm", model);
        }
    }
}
