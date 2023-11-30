using Forum.Web.Models.User;
using System.Security.Claims;

namespace Forum.Web.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetToken(this ClaimsPrincipal user)
        {
            var token = user.Claims.FirstOrDefault(x => x.Type == "JWTToken")?.Value;
            return token ?? string.Empty;
        }

        public static int GetUserId(this ClaimsPrincipal user)
        {
            var stringId = user.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;
            int userId = string.IsNullOrEmpty(stringId) ? 0 : int.Parse(stringId);
            return userId; 
        }
    }
}
