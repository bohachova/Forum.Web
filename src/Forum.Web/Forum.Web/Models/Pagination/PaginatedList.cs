namespace Forum.Web.Models.Pagination
{
    public class PaginatedList<T>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public List<T> Items { get; set; } = new List<T>();
        public bool HasPreviousPage;
        public bool HasNextPage;
    }
}
