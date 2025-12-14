using System.ComponentModel.DataAnnotations;

namespace Bookstore.Api.DTOs
{
    public class UpdateBookcaseDto
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Location { get; set; }
        public List<int> BookIds { get; set; } = [];
    }
}
