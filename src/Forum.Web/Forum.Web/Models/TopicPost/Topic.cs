using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace Forum.Web.Models.TopicPost
{
    public class Topic
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;
        public List<Post> Posts { get; set; } = new List<Post>();
        [Required]
        public int AuthorId { get; set; }
    }
}
