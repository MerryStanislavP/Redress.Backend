using MediatR;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Contracts.DTOs.AuthDTOs;
using Redress.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Redress.Backend.Application.Services.Auth.Login
{
    public class LoginCommand : IRequest<AuthResponseDto>
    {
        public LoginDto LoginDto { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
    {
        private readonly IRedressDbContext _context;
        private readonly IJwtService _jwtService;

        public LoginCommandHandler(IRedressDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.LoginDto.Email, cancellationToken);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid email or password");

            if (!VerifyPassword(request.LoginDto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password");

            // Generate tokens
            var newAccessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // Update user's refresh token
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);
            user.LastLoginAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                User = new UserInfoDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.Username,
                    Role = (Redress.Backend.Contracts.DTOs.Enums.UserRole)user.Role,
                    IsEmailConfirmed = user.IsEmailConfirmed
                }
            };
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
} 