using AutoMapper;
using Bookstore.Api.DTOs;
using Bookstore.App.Services.Interfaces;
using Bookstore.Domain.Entities;
using Bookstore.Infrastructure.Repositories.Interfaces;
using Bookstore.Infrastructure.Security;

namespace Bookstore.App.Services
{
    public class AuthService(
        IUserRepository userRepo,
        IUnitOfWork uow,
        IMapper mapper,
        JwtTokenGenerator jwt) : IAuthService
    {
        private readonly IUserRepository _userRepo = userRepo;
        private readonly IUnitOfWork _uow = uow;
        private readonly IMapper _mapper = mapper;
        private readonly JwtTokenGenerator _jwt = jwt;

        public async Task RegisterAsync(RegisterDto dto)
        {
            if (await _userRepo.GetByEmailAsync(dto.Email) != null)
                throw new Exception("Email already registered.");

            var user = _mapper.Map<User>(dto);

            await _userRepo.AddAsync(user);
            await _uow.SaveChangesAsync();
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email)
                ?? throw new Exception("Invalid credentials.");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials.");

            return _jwt.GenerateToken(user);
        }
    }
}
