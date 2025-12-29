namespace Bookstore.App.Filters
{
    public class BookFilter: PaginationFilterBase
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
        public bool? IsRead { get; set; }
        public int UserId { get; set; }
    }
}
