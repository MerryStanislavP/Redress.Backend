
namespace Redress.Backend.Contracts.DTOs.AuthDTOs
{
    public class ExternalLoginDto
    {
        public string Provider { get; set; }
        public string Token { get; set; }
    }
}
