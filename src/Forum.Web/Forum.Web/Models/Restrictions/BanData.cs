using Forum.Web.Enums;

namespace Forum.Web.Models.Restrictions
{
    public class BanData
    {
        public int UserId { get; set; }
        public BanType BanType { get; set; }
        public TimeSpan? BanTime { get; set; }
    }
}
