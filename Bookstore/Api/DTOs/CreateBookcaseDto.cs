using System.ComponentModel.DataAnnotations;
using Bookstore.Api.Models;

namespace Bookstore.Api.DTOs
{
    public class CreateBookcaseDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Location { get; set; }
        public List<int> BookIds { get; set; } = [];
    }
}
