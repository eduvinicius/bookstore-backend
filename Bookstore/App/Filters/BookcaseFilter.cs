namespace Bookstore.App.Filters
{
    public class BookcaseFilter: PaginationFilterBase
    {
        public string? Name { get; set; }
        public int UserId { get; set; }
    }
}
