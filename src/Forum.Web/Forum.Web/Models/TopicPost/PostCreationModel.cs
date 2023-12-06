using System.ComponentModel.DataAnnotations;

namespace Forum.Web.Models.TopicPost
{
    public class PostCreationModel
    {
        [Required]
        [StringLength(150)]
        public string Header { get; set; } = string.Empty;
        [Required]
        [StringLength(1000)]
        public string Text { get; set; } = string.Empty;
        public List<IFormFile> Attachments { get; set; } = new List<IFormFile>();
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public int TopicId { get; set; }
    }
}
