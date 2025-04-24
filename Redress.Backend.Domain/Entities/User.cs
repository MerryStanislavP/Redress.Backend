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
    }
}
