using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Domain.Entities;
using Redress.Backend.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Redress.Backend.Infrastructure.Integration.Authentication
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly GoogleAuthOptions _options;
        private readonly IRedressDbContext _context;
        private readonly IJwtService _jwtService;

        public GoogleAuthService(
            IOptions<GoogleAuthOptions> options,
            IRedressDbContext context,
            IJwtService jwtService)
        {
            _options = options.Value;
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<(string accessToken, string refreshToken)> ValidateGoogleTokenAsync(string idToken, CancellationToken cancellationToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new[] { _options.ClientId }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

                // Check if user exists
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == payload.Email, cancellationToken);

                if (user == null)
                {
                    // Create new user
                    user = new User
                    {
                        Email = payload.Email,
                        Username = payload.Name,
                        IsEmailConfirmed = true,
                        Provider = "Google",
                        ProviderKey = payload.Subject,
                        LastLoginAt = DateTime.UtcNow
                    };

                    await _context.Users.AddAsync(user, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    // Update last login
                    user.LastLoginAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync(cancellationToken);
                }

                // Generate JWT tokens
                var accessToken = _jwtService.GenerateAccessToken(user);
                var refreshToken = _jwtService.GenerateRefreshToken();

                // Save refresh token
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _context.SaveChangesAsync(cancellationToken);

                return (accessToken, refreshToken);
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid Google token", ex);
            }
        }
    }
} 