using Forum.Web.Configuration.Interfaces;
using Forum.Web.Extensions;
using Forum.Web.Interfaces;
using Forum.Web.Models.Pagination;
using Forum.Web.Models.Restrictions;
using Forum.Web.Models.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    public class UserProfilesController : Controller
    {
        private readonly IForumAPI forumAPI;
        private readonly IPaginationSettingsConfiguration settings;

        public UserProfilesController(IForumAPI forumAPI, IPaginationSettingsConfiguration settings)
        {
            this.forumAPI = forumAPI;
            this.settings = settings;
        }
        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllProfiles(int pageNumber = 1)
        {
            var token = User.GetToken();
            var result = await forumAPI.GetAllProfiles(new PaginationSettings { PageNumber = pageNumber, PageSize = settings.UsersPageSize }, token);
            return View("Users", result);
        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetUserProfile(int userId)
        {
            var token = User.GetToken();
            var user = await forumAPI.GetUserProfile(userId, token);
            return View("UserProfile", user);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyProfile()
        {
            var token = User.GetToken();
            var userId = User.GetUserId();
            var user = await forumAPI.GetUserProfile(userId, token);
            return View("UserProfile", user);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditUserProfile(UserModel model)
        {
            var token = User.GetToken();
            var user = await forumAPI.EditUserProfile(model, token);
            if(user != null)
            {
                return View("UserProfile", user);
            }
            else
            {
                ViewBag.ErrorMessage = "Incorect data entered";
                return View("UserProfile", model);
            }
        }
        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<IActionResult> ViewUserPhoto(int userId)
        {
            var token = User.GetToken();
            var user = await forumAPI.GetUserProfile(userId, token);
            ViewBag.UserId = userId;
            ViewBag.UserPhoto = user.UserPhoto;
            return View("UserPhoto");
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ViewMyPhoto()
        {
            var token = User.GetToken();
            var userId = User.GetUserId();
            var user = await forumAPI.GetUserProfile(userId, token);
            ViewBag.UserPhoto = user.UserPhoto;
            return View("UserPhoto");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SetMyPhoto(IFormFile file)
        {
            if (file != null)
            {
                var token = User.GetToken();
                var userId = User.GetUserId();
                var result = await forumAPI.SetUserPhoto(userId,file.OpenReadStream(), file.FileName, file.ContentType, token);
                if (result.IsSuccess)
                    return RedirectToAction("GetMyProfile");
                else
                {
                    ViewBag.Error = result.Message;
                    return View("UserPhoto");
                }
            }
            else
            {
                ViewBag.Error = "Please, load file";
                return View("UserPhoto");
            }
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteUserPhoto([FromForm]int userId)
        {
            var token = User.GetToken();
            var result = forumAPI.DeleteUserPhoto(userId, token);
            return RedirectToAction("GetUserProfile", new {userId});
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DeleteMyPhoto()
        {
            var token = User.GetToken();
            var userId = User.GetUserId();
            await forumAPI.DeleteUserPhoto(userId, token);
            return RedirectToAction("GetMyProfile");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteUser (int userId)
        {
            var token = User.GetToken();
            await forumAPI.DeleteUser(userId, token);
            return RedirectToAction("GetAllProfiles");
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DeleteMyProfile()
        {
            var token = User.GetToken();
            var userId = User.GetUserId();
            await forumAPI.DeleteUser(userId, token);
            return RedirectToAction("Logout", "Authorization");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> BanUser(int userId, int banType, int? banTime)
        {
            var token = User.GetToken();
            if(banTime != null)
            {
                var time = new TimeSpan((int)banTime, 0, 0, 0);
                await forumAPI.BanUser(new BanData { UserId = userId, BanType = (Enums.BanType)banType, BanTime = time }, token);
            }
            else
            {
                await forumAPI.BanUser(new BanData { UserId = userId, BanType = (Enums.BanType)banType}, token);
            }
            return RedirectToAction("GetUserProfile", new { userId });
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> UnbanUser(int userId)
        {
            var token = User.GetToken();
            var result = await forumAPI.UnbanUser(userId, token);
            return RedirectToAction("GetUserProfile", new {userId });
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetBannedUsersList (int pageNumber)
        {
            var token = User.GetToken();
            var result = await forumAPI.GetBannedUsersList(new PaginationSettings { PageNumber = pageNumber, PageSize = settings.UsersPageSize }, token);
            return View("BannedUsers", result);
        }

    }
}
