using Redress.Backend.Contracts.DTOs.Enums;

namespace Redress.Backend.Contracts.DTOs.ReadingDTO
{
    public class DealDto
    {
        public Guid Id { get; set; }
        public DealStatus Status { get; set; }
        public DealType ListingType { get; set; }
        public decimal Price { get; set; }
    }
}
