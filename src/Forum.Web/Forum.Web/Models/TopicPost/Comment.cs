using Forum.Web.Models.Pagination;
using Forum.Web.Models.User;
using System.ComponentModel.DataAnnotations;

namespace Forum.Web.Models.TopicPost
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int ParentId { get; set; }
        public int AuthorId { get; set; }
        public UserModel? Author { get; set; }
        public Comment? Parent { get; set; }
        [Required]
        [StringLength(500)]
        public string Text { get; set; } = string.Empty;
        public DateTime PublishingTime { get; set; } = DateTime.Now;
        public bool HasChildComments { get; set; }
        public int ParentAuthorId { get; set; }
        public CurrentPaginationPositionSettings? Position { get; set; }
    }
}
