using Forum.Web.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Forum.Web.Models.Pagination;

namespace Forum.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IForumAPI forumAPI;
        public HomeController(IForumAPI forumAPI)
        {
            this.forumAPI = forumAPI;
        }
        public async Task<IActionResult> Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("ForumMainPage");
            }
            var topics = await forumAPI.GetTopicsList(new PaginationSettings { PageNumber = 1 });
            return View(topics);
        }
        [Authorize]
        public async Task<IActionResult> ForumMainPage(int pageNumber = 1)
        {
            var topics = await forumAPI.GetTopicsList(new PaginationSettings { PageNumber = pageNumber });
            return View(topics);
        }
    }
}
