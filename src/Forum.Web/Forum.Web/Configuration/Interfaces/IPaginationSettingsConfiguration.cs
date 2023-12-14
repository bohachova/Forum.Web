namespace Forum.Web.Configuration.Interfaces
{
    public interface IPaginationSettingsConfiguration
    {
        public int UsersPageSize { get; set; }
        public int TopicsPageSize { get; set; }
        public int PostsPageSize { get; set; }
    }
}
