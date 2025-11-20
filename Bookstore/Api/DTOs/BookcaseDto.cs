namespace Bookstore.Api.DTOs
{
    public class BookcaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Location { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<BookDto> Books { get; set; } = [];
    }
}
