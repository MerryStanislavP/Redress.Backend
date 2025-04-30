

namespace Redress.Backend.Contracts.DTOs.CreateDTO
{
    public class FavoriteCreateDto
    {
        public Guid ProfileId { get; set; }
        public Guid ListingId { get; set; }
    }
}
