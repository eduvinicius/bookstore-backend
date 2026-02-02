using AutoMapper;
using Bookstore.Api.DTOs;
using Bookstore.App.Services.Interfaces;
using Bookstore.Domain.Entities;
using Bookstore.Infrastructure.Repositories.Interfaces;
using Bookstore.Infrastructure.Security;
using Bookstore.Domain.Exceptions;

namespace Bookstore.App.Services
{
    public class AuthService(
        IUserRepository userRepo,
        IUnitOfWork uow,
        IMapper mapper,
        ILogger<AuthService> logger,
        JwtTokenGenerator jwt) : IAuthService
    {
        private readonly IUserRepository _userRepo = userRepo;
        private readonly IUnitOfWork _uow = uow;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<AuthService> _logger = logger;
        private readonly JwtTokenGenerator _jwt = jwt;

        public async Task RegisterAsync(RegisterDto dto)
        {
            _logger.LogInformation("Attempting to register user with email: {Email}", dto.Email);

            if (await _userRepo.GetByEmailAsync(dto.Email) != null)
            {
                _logger.LogWarning("Registration failed: Email {Email} already exists", dto.Email);
                throw new ConflictException("Email already registered.");
            }

            var user = _mapper.Map<User>(dto);

            await _userRepo.AddAsync(user);
            await _uow.SaveChangesAsync();

            _logger.LogInformation("User {UserId} registered successfully with email: {Email}", user.Id, dto.Email);
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            _logger.LogInformation("Login attempt for email: {Email}", dto.Email);

            var user = await _userRepo.GetByEmailAsync(dto.Email)
                ?? throw new BadRequestException("Invalid credentials.");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                _logger.LogWarning("Failed login attempt for email: {Email} - Invalid password", dto.Email);
                throw new BadRequestException("Invalid credentials.");
            }

            var token = _jwt.GenerateToken(user);
            _logger.LogInformation("User {UserId} logged in successfully", user.Id);

            return token;
        }
    }
}
