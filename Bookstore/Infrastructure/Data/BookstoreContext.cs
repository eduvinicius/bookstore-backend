using Bookstore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Infrastructure.Data
{
    public class BookstoreContext(DbContextOptions<BookstoreContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Bookcase>()
                .HasOne(bc => bc.User)
                .WithMany(u => u.Bookcases)
                .HasForeignKey(bc => bc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.User)
                .WithMany(u => u.Books)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Bookcase)
                .WithMany(bc => bc.Books)
                .HasForeignKey(b => b.BookcaseId)
                .OnDelete(DeleteBehavior.SetNull);
        }


        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Bookcase> Bookcases { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
    }
}
