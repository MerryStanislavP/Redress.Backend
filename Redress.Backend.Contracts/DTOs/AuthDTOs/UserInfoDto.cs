using Redress.Backend.Contracts.DTOs.Enums;

namespace Redress.Backend.Contracts.DTOs.AuthDTOs
{
    public class UserInfoDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public UserRole Role { get; set; }
        public bool IsEmailConfirmed { get; set; }
    }
}
