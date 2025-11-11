namespace Bookstore.Api.DTOs
{
    public class UpdateBookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int? BookcaseId { get; set; }
        public DateOnly? ReadDate { get; set; }
        public DateOnly? StartedReadingDate { get; set; }
        public double? Rating { get; set; }
        public int? PageCount { get; set; }
        public string? Language { get; set; }
        public string? Description { get; set; }
    }
}
