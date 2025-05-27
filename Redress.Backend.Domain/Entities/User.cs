using Redress.Backend.Domain.Enums;

namespace Redress.Backend.Domain.Entities
{ 
    public class User // користувач
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string PasswordHash { get; set; } = string.Empty!;
        public UserRole Role { get; set; } = UserRole.Regular;
        public Sex? Sex { get; set; }
        public Profile? Profile { get; set; } // нав. свойство

        // Поля для внешней авторизации
        public string? ExternalId { get; set; }
        public string? Provider { get; set; }
        public string? ProviderKey { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
