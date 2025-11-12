using System.ComponentModel.DataAnnotations;
using Bookstore.Api.Models;

namespace Bookstore.Api.DTOs
{
    public class CreateBookcaseDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Location { get; set; }
        public List<Book> Books { get; set; } = [];
    }
}
