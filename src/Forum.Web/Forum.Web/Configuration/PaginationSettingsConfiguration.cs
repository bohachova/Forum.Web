using Forum.Web.Configuration.Interfaces;

namespace Forum.Web.Configuration
{
    public class PaginationSettingsConfiguration : IPaginationSettingsConfiguration
    {
        public int UsersPageSize { get; set; }
        public int TopicsPageSize { get; set; }
        public int PostsPageSize { get; set; }
    }
}
