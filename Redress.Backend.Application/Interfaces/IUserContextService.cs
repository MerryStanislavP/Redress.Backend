using System.Security.Claims;

namespace Redress.Backend.Application.Interfaces
{
    public interface IUserContextService
    {
        Guid? GetCurrentUserId();
        ClaimsPrincipal? GetCurrentUser();
        bool IsAuthenticated();
    }
} 