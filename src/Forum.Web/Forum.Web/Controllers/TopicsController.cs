using Forum.Web.Extensions;
using Forum.Web.Interfaces;
using Forum.Web.Models.TopicPost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Forum.Web.Models.Pagination;

namespace Forum.Web.Controllers
{
    public class TopicsController : Controller
    {
        private readonly IForumAPI forumAPI;
        public TopicsController(IForumAPI forumAPI)
        {
            this.forumAPI = forumAPI;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetTopicPosts(int pageNumber, int topicId, string topicName)
        {
            var token = User.GetToken();
            var result = await forumAPI.GetTopicPosts(new PaginationSettings { PageNumber = pageNumber}, topicId, token);
            ViewBag.TopicId = topicId;
            ViewBag.TopicName = topicName;
            return View("Posts", result);
        }
        [Authorize(Roles ="Admin")]
        [HttpGet]
        public IActionResult CreateTopic()
        {
            return View("NewTopicForm");
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateTopic(Topic topic)
        {
            if(ModelState.IsValid)
            {
                var token = User.GetToken();
                var result = await forumAPI.CreateTopic(topic, token);
                if (result.IsSuccess)
                {
                    ViewBag.Success = "New topic was created!";
                }
            }
            ViewBag.Error = "Creation failed!";
            return View("NewTopicForm", topic);

        }
        [Authorize]
        [HttpGet]
        public IActionResult CreatePost()
        {
            return PartialView("NewPostForm");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePost(PostCreationModel post)
        {
            var token = User.GetToken();
            if(ModelState.IsValid)
            {
                var result = await forumAPI.CreatePost(post, token);
                if (result.IsSuccess)
                {
                    ViewBag.Success = "New post was created!";
                }
            }
            ViewBag.Error = "Creation failed!";
            return PartialView("NewPostForm", post);
        }
    }
}
