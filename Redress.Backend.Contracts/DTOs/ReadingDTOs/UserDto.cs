using Redress.Backend.Contracts.DTOs.Enums;

namespace Redress.Backend.Contracts.DTOs.ReadingDTOs
{
    public class UserDto 
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public string PhoneNumber {  get; set; }
        public string PasswordHash { get; set; }
    }
}
