namespace Forum.Web.Models.Pagination
{
    public class CurrentPaginationPositionSettings
    {
        public int PageNumber { get; set; } 
        public int TopicId { get; set; }
        public string TopicTitle { get; set; } = string.Empty;
        public int PostId { get; set; }
    }
}
