using Bookstore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Infrastructure.Data
{
    public class BookstoreContext(DbContextOptions<BookstoreContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Bookcase)
                .WithMany(bc => bc.Books)
                .HasForeignKey(b => b.BookcaseId)
                .OnDelete(DeleteBehavior.SetNull);
        }


        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Bookcase> Bookcases { get; set; } = null!;
    }
}
