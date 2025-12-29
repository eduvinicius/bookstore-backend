using AutoMapper;
using Bookstore.Api.DTOs;
using Bookstore.App.Services.Interfaces;
using Bookstore.Domain.Entities;

namespace Bookstore.App.Services
{
    public class BookImportService(
        IGoogleBooksService googleBooksService,
        IBookService bookService,
        IMapper mapper) : IBookImportService
    {
        private readonly IGoogleBooksService _googleBooksService = googleBooksService;
        private readonly IBookService _bookService = bookService;
        private readonly IMapper _mapper = mapper;

        public async Task<Book> ImportFromGoogleAsync(string googleBookId, int? bookcaseId, int userId)
        {
            var googleBook = await _googleBooksService.GetByIdAsync(googleBookId) ?? throw new Exception("Book not found on Google");

            var createBookDto = _mapper.Map<CreateBookDto>(googleBook);
            createBookDto.UserId = userId;

            if (bookcaseId != null)
                createBookDto.BookcaseId = bookcaseId;

            return await _bookService.CreateBookAsync(createBookDto);
        }
    }

}
