using Redress.Backend.Domain.Enums;

namespace Redress.Backend.Domain.Entities
{ 
    public class User // користувач
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public Profile Profile { get; set; }
        public Sex Sex { get; set; }
    }
}
