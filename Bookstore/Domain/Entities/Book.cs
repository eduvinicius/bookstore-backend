namespace Bookstore.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int Year { get; set; }
        public DateTime CreatedDate {  get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsRead { get; set; }
        public int? PageCount { get; set; }
        public string? Language { get; set; }
        public DateOnly? ReadDate { get; set; } 
        public DateOnly? StartedReadingDate { get; set; }
        public double? Rating { get; set; }
        public string? Description { get; set; }
        public string? ThumbnailUrl { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; } = null!;
        public int? BookcaseId { get; set; }
        public Bookcase? Bookcase { get; set; }

    }
}
