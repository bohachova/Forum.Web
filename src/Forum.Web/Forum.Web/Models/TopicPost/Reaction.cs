namespace Forum.Web.Models.TopicPost
{
    public class Reaction
    {
        public int AuthorId { get; set; }
        public bool Like { get; set; } = false;
        public bool Dislike { get; set; } = false;
        public int TargetId { get; set; }
    }
}
