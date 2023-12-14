using Forum.Web.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Forum.Web.Models.Pagination;
using Forum.Web.Configuration.Interfaces;

namespace Forum.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IForumAPI forumAPI;
        private readonly IPaginationSettingsConfiguration settings;
        public HomeController(IForumAPI forumAPI, IPaginationSettingsConfiguration settings)
        {
            this.forumAPI = forumAPI;
            this.settings = settings;
        }
        public async Task<IActionResult> Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("ForumMainPage");
            }
            var topics = await forumAPI.GetTopicsList(new PaginationSettings { PageNumber = 1, PageSize = settings.TopicsPageSize });
            return View(topics);
        }
        [Authorize]
        public async Task<IActionResult> ForumMainPage(int pageNumber = 1)
        {
            var topics = await forumAPI.GetTopicsList(new PaginationSettings { PageNumber = pageNumber, PageSize = settings.TopicsPageSize });
            return View(topics);
        }
    }
}
