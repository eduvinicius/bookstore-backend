using AutoMapper;
using Bookstore.Api.DTOs;
using Bookstore.Api.Models;
using Bookstore.Repositories;
using Bookstore.Repositories.Interfaces;
using Bookstore.Services.Interfaces;

namespace Bookstore.Services
{
    public class BookcasesService: IBookcasesService
    {
        private readonly IBookcaseRepository _bookcaseRepository;
        private readonly IMapper _mapper;

        public BookcasesService(IBookcaseRepository bookcaseRepository, IMapper mapper)
        {
            _bookcaseRepository = bookcaseRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Bookcase>> GetAllBookcasesAsync()
        {
            return await _bookcaseRepository.GetAllAsync();
        }

        public async Task<Bookcase> CreateBookcaseAsync(CreateBookcaseDto dto)
        {
            var bookcase = new Bookcase();
            _mapper.Map(dto, bookcase);

            await _bookcaseRepository.AddAsync(bookcase);
            await _bookcaseRepository.SaveChangesAsync();

            return bookcase;
        }

        public async Task<Bookcase> GetBookcaseByIdAsync(int id)
        {
            var bookcase = await _bookcaseRepository.GetByIdAsync(id);

            return bookcase ?? throw new KeyNotFoundException($"Bookcase with ID {id} not found.");
        }

        public async Task<Bookcase> UpdateBookcaseAsync(int id, UpdateBookcaseDto dto)
        {
            var bookcase = await _bookcaseRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Book with ID {id} not found.");


            _mapper.Map(dto, bookcase);
            _bookcaseRepository.Update(bookcase);
            await _bookcaseRepository.SaveChangesAsync();
            return bookcase;
        }

        public async Task<bool> DeleteBookcaseAsync(int id)
        {
            var bookcase = await _bookcaseRepository.GetByIdAsync(id);
            if (bookcase == null)
            {
                return false;
            }
            _bookcaseRepository.Delete(bookcase);
            await _bookcaseRepository.SaveChangesAsync();
            return true;
        }
    }
}
