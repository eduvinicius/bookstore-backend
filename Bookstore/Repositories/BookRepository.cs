using Bookstore.Api.Data;
using Bookstore.Api.Models;
using Bookstore.Api.DTOs;
using Bookstore.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Repositories
{
    public class BookRepository: IBookRepository
    {
        private readonly BookstoreContext _context;

        public BookRepository(BookstoreContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Book book)
        {
            await _context.Books.AddAsync(book);
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books
                .Include(b => b.Bookcase)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> GetByBookcaseIdAsync(int id)
        {
            return await _context.Books
                .Where(b => b.BookcaseId == id)
                .Include(b => b.Bookcase)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAllAsync(int skip, int take)
        {
            return await _context.Books
                .OrderBy(b => b.Id)
                .Skip(skip)
                .Take(take)
                .Include(b => b.Bookcase)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetUnassignedBooksAsync()
        {
            return await _context.Books
                .Where(b => b.BookcaseId == null)
                .Include(b => b.Bookcase)
                .ToListAsync();
        }

        public async Task<List<Book>> GetByIdsListAsync(List<int> ids)
        {
            return await _context.Books
                .Where(b => ids.Contains(b.Id))
                .Include(b => b.Bookcase)
                .ToListAsync();
        }


        public void Update(Book book)
        {
            _context.Books.Update(book);
        }

        public void Delete(Book book)
        {
            _context.Books.Remove(book);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
