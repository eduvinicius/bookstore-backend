using System.ComponentModel.DataAnnotations;

namespace Bookstore.Domain.Entities
{
    public class Bookcase
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Location { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        public int? UserId { get; set; }
        public User? User { get; set; } = null!;
        public List<Book> Books { get; set; } = [];
    }
}
