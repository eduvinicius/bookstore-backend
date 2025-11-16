using Bookstore.Api.Data;
using Bookstore.Api.Models;
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
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> GetByBookcaseIdAsync(int id)
        {
            return await _context.Books
                .Where(b => b.BookcaseId == id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAllAsync(int skip, int take)
        {
            return await _context.Books
                .OrderBy(b => b.Id)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetUnassignedBooksAsync()
        {
            return await _context.Books
                .Where(b => b.BookcaseId == null)
                .ToListAsync();
        }

        public async Task<List<Book>> GetByIdsListAsync(List<int> ids)
        {
            return await _context.Books
                .Where(b => ids.Contains(b.Id))
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
