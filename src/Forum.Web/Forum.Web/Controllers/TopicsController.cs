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
        public async Task<IActionResult> GetTopicPosts(CurrentPaginationPositionSettings position)
        {
            var token = User.GetToken();
            var result = await forumAPI.GetTopicPosts(new PaginationSettings { PageNumber = position.PageNumber, PageSize = settings.PostsPageSize}, position.TopicId, token);
            ViewBag.TopicId = position.TopicId;
            ViewBag.TopicTitle = position.TopicTitle;
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
        public async Task<IActionResult> CreatePost(PostCreationModel post, CurrentPaginationPositionSettings position)
        {
            var token = User.GetToken();
            if(ModelState.IsValid)
            {
                var result = await forumAPI.CreatePost(post, token);
                if (result.IsSuccess)
                {
                    return RedirectToAction("GetTopicPosts", new { PageNumber = position.PageNumber, TopicId = position.TopicId, TopicTitle=position.TopicTitle});
                }
            }
            return BadRequest();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeletePost (int postId, bool commentsToPost, int authorId, CurrentPaginationPositionSettings position)
        {
            var userId = User.GetUserId();
            if(User.IsInRole("Admin"))
            {
                var token = User.GetToken();
                await forumAPI.DeletePost(postId, token);
                return RedirectToAction("GetTopicPosts", new { PageNumber = 1, TopicId = position.TopicId, TopicTitle = position.TopicTitle });
            }
            else if(User.IsInRole("User") && !commentsToPost && userId == authorId )
            {
                var token = User.GetToken() ;
                await forumAPI.DeletePost(postId, token);
                return RedirectToAction("GetTopicPosts", new { PageNumber = 1, TopicId = position.TopicId, TopicTitle = position.TopicTitle });
            }
            return BadRequest();
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ViewPost (CurrentPaginationPositionSettings position)
        {
            var token = User.GetToken();
            var post = await forumAPI.ViewPost(position.PostId, new PaginationSettings { PageNumber = position.PageNumber, PageSize = settings.PostsPageSize}, token);
            ViewBag.TopicTitle = position.TopicTitle;
            return View("PostPage", post);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditPost(PostEditModel post, CurrentPaginationPositionSettings position)
        {
            var token = User.GetToken();
            await forumAPI.EditPost(post, token);
            return RedirectToAction("GetTopicPosts", new { PageNumber = position.PageNumber, TopicId = position.TopicId, TopicTitle = position.TopicTitle });
        }

    }
}
