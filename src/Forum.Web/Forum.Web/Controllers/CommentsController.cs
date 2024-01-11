using Forum.Web.Extensions;
using Forum.Web.Interfaces;
using Forum.Web.Models.Pagination;
using Forum.Web.Models.TopicPost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    public class CommentsController : Controller
    {
        private readonly IForumAPI forumAPI;
        public CommentsController(IForumAPI forumAPI)
        {
            this.forumAPI = forumAPI;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> LeaveComment (Comment comment, CurrentPaginationPositionSettings position)
        {
            comment.Position = position;
            if (ModelState.IsValid)
            {
                var token = User.GetToken();
                await forumAPI.LeaveComment(comment, token);
            }
            return RedirectToAction("ViewPost", "Topics", new {PageNumber = position.PageNumber, TopicId = position.TopicId, TopicTitle = position.TopicTitle, PostId = position.PostId });
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteComment(int commentId, bool childComments, int authorId, CurrentPaginationPositionSettings position)
        {
            var userId = User.GetUserId();
            if (User.IsInRole("Admin"))
            {
                var token = User.GetToken();
                await forumAPI.DeleteComment(commentId, token);
                return RedirectToAction("GetTopicPosts", new { PageNumber = position.PageNumber, TopicId = position.TopicId, TopicTitle = position.TopicTitle, PostId = position.PostId });
            }
            else if (User.IsInRole("User") && !childComments && userId == authorId)
            {
                var token = User.GetToken();
                await forumAPI.DeleteComment(commentId, token);
                return RedirectToAction("GetTopicPosts", new { PageNumber = position.PageNumber, TopicId = position.TopicId, TopicTitle = position.TopicTitle, PostId = position.PostId });
            }
            return BadRequest();
        }
    }
}
