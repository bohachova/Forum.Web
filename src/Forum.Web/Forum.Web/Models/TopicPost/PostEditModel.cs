using System.ComponentModel.DataAnnotations;

namespace Forum.Web.Models.TopicPost
{
    public class PostEditModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Header { get; set; } = string.Empty;
        [Required]
        [StringLength(1000)]
        public string Text { get; set; } = string.Empty;
        public List<IFormFile> NewAttachments { get; set; } = new List<IFormFile>();
        public string DeletedAttachments { get; set; } = string.Empty;
    }
}
