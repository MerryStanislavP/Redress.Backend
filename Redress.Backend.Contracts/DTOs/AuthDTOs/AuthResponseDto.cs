
namespace Redress.Backend.Contracts.DTOs.AuthDTOs
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public UserInfoDto User { get; set; }
    }
}
