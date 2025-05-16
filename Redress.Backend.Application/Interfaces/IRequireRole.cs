using Redress.Backend.Domain.Enums;

namespace Redress.Backend.Application.Interfaces
{
    public interface IRequireRole
    {
        Guid UserId { get; }
        UserRole RequiredRole { get; }
    }
}
