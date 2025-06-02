namespace Redress.Backend.Application.Interfaces
{
    public interface IGoogleAuthService
    {
        Task<(string accessToken, string refreshToken)> ValidateGoogleTokenAsync(string idToken, CancellationToken cancellationToken);
    }
} 