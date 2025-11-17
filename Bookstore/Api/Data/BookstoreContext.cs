using Bookstore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Api.Data
{
    public class BookstoreContext : DbContext
    {
        public BookstoreContext(DbContextOptions<BookstoreContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne<Bookcase>()
                .WithMany(bc => bc.Books)
                .HasForeignKey(b => b.BookcaseId)
                .OnDelete(DeleteBehavior.SetNull);
        }


        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Bookcase> Bookcases { get; set; } = null!;
    }
}
