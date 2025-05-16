using Redress.Backend.Domain.Enums;

namespace Redress.Backend.Application.Interfaces
{
    /// <summary>
    /// Интерфейс для проверки наличия у пользователя одной из требуемых ролей.
    /// Запросы, реализующие этот интерфейс, будут проверяться на наличие у пользователя одной из требуемых ролей.
    /// </summary>
    public interface IRequireAnyRole
    {
        /// <summary>
        /// ID пользователя, выполняющего запрос
        /// </summary>
        Guid UserId { get; }
        
        /// <summary>
        /// Массив ролей, одна из которых требуется для выполнения запроса
        /// </summary>
        UserRole[] RequiredRoles { get; }
    }
} 