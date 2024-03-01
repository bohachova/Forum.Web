using System.ComponentModel.DataAnnotations;
using Forum.Web.Models.Pagination;
using Forum.Web.Models.User;

namespace Forum.Web.Models.TopicPost
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Header { get; set; } = string.Empty;
        [Required]
        [StringLength(1000)]
        public string Text { get; set; } = string.Empty;
        public List<Attachment> Attachments { get; set; } = new List<Attachment>();
        public DateTime PostPublishingTime { get; set; }
        [Required]
        public int AuthorId { get; set; }
        public UserModel Author { get; set; }
        [Required]
        public int TopicId { get; set; }
        public PaginatedList<Comment> Comments { get; set; }
        public bool WasEdited { get; set; }
        public DateTime? LastEdited { get; set; }
        public List<Reaction> Reactions { get; set; } = new List<Reaction>();
    }
}
