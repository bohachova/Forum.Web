namespace Forum.Web.Models.TopicPost
{
    public class Attachment
    {
        public int Id { get; set; }
        public string File { get; set; }
        public int PostId { get; set; }
    }
}
