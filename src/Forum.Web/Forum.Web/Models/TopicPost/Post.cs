using System.ComponentModel.DataAnnotations;

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
        public List<string> Attachments { get; set; } = new List<string>();
        public DateTime PostPublishingTime { get; set; } = DateTime.Now;
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public int TopicId { get; set; }
    }
}
