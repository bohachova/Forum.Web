using System.ComponentModel.DataAnnotations;

namespace Forum.Web.Models.TopicPost
{
    public class CommentEditModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(500)]
        public string Text { get; set; } = string.Empty;
    }
}
