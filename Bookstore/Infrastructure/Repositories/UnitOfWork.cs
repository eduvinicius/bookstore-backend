using Bookstore.Infrastructure.Data;
using Bookstore.Infrastructure.Repositories.Interfaces;

namespace Bookstore.Infrastructure.Repositories
{
    public class UnitOfWork(
        BookstoreContext context,
        IBookRepository bookRepository,
        IBookcaseRepository bookcaseRepository) : IUnitOfWork
    {
        private readonly BookstoreContext _context = context;

        public IBookRepository Books { get; } = bookRepository;
        public IBookcaseRepository Bookcases { get; } = bookcaseRepository;

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }

}
