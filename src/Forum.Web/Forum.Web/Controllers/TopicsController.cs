using Forum.Web.Extensions;
using Forum.Web.Interfaces;
using Forum.Web.Models.TopicPost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Forum.Web.Models.Pagination;
using Forum.Web.Configuration.Interfaces;

namespace Forum.Web.Controllers
{
    public class TopicsController : Controller
    {
        private readonly IForumAPI forumAPI;
        private readonly IPaginationSettingsConfiguration settings;
        public TopicsController(IForumAPI forumAPI, IPaginationSettingsConfiguration settings)
        {
            this.forumAPI = forumAPI;
            this.settings = settings;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetTopicPosts(int pageNumber, int topicId, string topicTitle)
        {
            var token = User.GetToken();
            var result = await forumAPI.GetTopicPosts(new PaginationSettings { PageNumber = pageNumber, PageSize = settings.PostsPageSize}, topicId, token);
            ViewBag.TopicId = topicId;
            ViewBag.TopicTitle = topicTitle;
            return View("Posts", result);
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateTopic(string topicTitle, int pageNumber)
        {
            if(ModelState.IsValid)
            {
                var token = User.GetToken();
                var userId = User.GetUserId();
                var result = await forumAPI.CreateTopic(new Topic { AuthorId = userId, Title = topicTitle}, token);
                if (result.IsSuccess)
                {
                    return RedirectToAction("ForumMainPage", "Home", new {pageNumber = pageNumber});
                }
            }
            return BadRequest();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePost(PostCreationModel post, int pageNumber, int topicId, string topicTitle)
        {
            var token = User.GetToken();
            if(ModelState.IsValid)
            {
                var result = await forumAPI.CreatePost(post, token);
                if (result.IsSuccess)
                {
                    return RedirectToAction("GetTopicPosts", new {pageNumber = pageNumber, topicId = topicId, topicTitle = topicTitle});
                }
            }
            return BadRequest();
        }
    }
}
