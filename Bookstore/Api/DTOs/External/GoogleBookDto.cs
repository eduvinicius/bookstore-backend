namespace Bookstore.Api.DTOs.External
{
    public class GoogleBookDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<string> Authors { get; set; } = [];
        public string? Description { get; set; }
        public string? Thumbnail { get; set; }
        public int? PublishedYear { get; set; }
        public int? PageCount { get; set; }
        public string? Language { get; set; }
    }
}
