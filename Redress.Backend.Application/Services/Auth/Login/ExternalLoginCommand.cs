using MediatR;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Contracts.DTOs.AuthDTOs;
using Redress.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Redress.Backend.Application.Services.Auth.Login
{
    public class ExternalLoginCommand : IRequest<AuthResponseDto>
    {
        public ExternalLoginDto ExternalLoginDto { get; set; }
    }

    public class ExternalLoginCommandHandler : IRequestHandler<ExternalLoginCommand, AuthResponseDto>
    {
        private readonly IRedressDbContext _context;
        private readonly IJwtService _jwtService;

        public ExternalLoginCommandHandler(IRedressDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> Handle(ExternalLoginCommand request, CancellationToken cancellationToken)
        {
            // Здесь должна быть валидация токена от провайдера (Google, Facebook и т.д.)
            // Для примера просто проверяем существование пользователя
            var user = await _context.Users
                .FirstOrDefaultAsync(u => 
                    u.Provider == request.ExternalLoginDto.Provider && 
                    u.ExternalId == request.ExternalLoginDto.Token, 
                    cancellationToken);

            if (user == null)
            {
                // Если пользователь не найден, создаем нового
                // В реальном приложении здесь нужно получить данные пользователя от провайдера
                user = new User
                {
                    Email = $"{request.ExternalLoginDto.Provider}@example.com", // Временный email
                    Username = $"{request.ExternalLoginDto.Provider}User",
                    Provider = request.ExternalLoginDto.Provider,
                    ExternalId = request.ExternalLoginDto.Token,
                    IsEmailConfirmed = true,
                    Role = Domain.Enums.UserRole.Regular
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync(cancellationToken);
            }

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
                ExpiresAt = DateTime.UtcNow.AddMinutes(180),
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
    }
} 