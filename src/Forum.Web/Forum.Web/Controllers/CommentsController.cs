using Forum.Web.Extensions;
using Forum.Web.Interfaces;
using Forum.Web.Models.Pagination;
using Forum.Web.Models.TopicPost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Forum.Web.Filters;

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
        [CheckUserAccessFilter]
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
                return RedirectToAction("ViewPost", "Topics", new { PageNumber = position.PageNumber, TopicId = position.TopicId, TopicTitle = position.TopicTitle, PostId = position.PostId });
            }
            else if (User.IsInRole("User") && !childComments && userId == authorId)
            {
                var token = User.GetToken();
                await forumAPI.DeleteComment(commentId, token);
                return RedirectToAction("ViewPost", "Topics", new { PageNumber = position.PageNumber, TopicId = position.TopicId, TopicTitle = position.TopicTitle, PostId = position.PostId });
            }
            return BadRequest();
        }
        [Authorize]
        [CheckUserAccessFilter]
        [HttpPost]
        public async Task<IActionResult> EditComment (CommentEditModel comment, CurrentPaginationPositionSettings position)
        {
            if (ModelState.IsValid)
            {
                var token = User.GetToken();
                await forumAPI.EditComment(comment, token);
            }
            return RedirectToAction("ViewPost", "Topics", new { PageNumber = position.PageNumber, TopicId = position.TopicId, TopicTitle = position.TopicTitle, PostId = position.PostId });
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CommentReaction(Reaction reaction)
        {
            var token = User.GetToken();
            var result = await forumAPI.CommentReaction(reaction, token);
            if (result.IsSuccess)
            {
                int likes = result.Likes;
                int dislikes = result.Dislikes;
                return Json(new { likes, dislikes });
            }
            else
                return null;
        }
    }
}

