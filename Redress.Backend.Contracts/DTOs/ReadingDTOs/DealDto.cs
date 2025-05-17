using Redress.Backend.Contracts.DTOs.Enums;

namespace Redress.Backend.Contracts.DTOs.ReadingDTOs
{
    public class DealDto
    {
        public Guid Id { get; set; }
        public DealStatus Status { get; set; }
        public DealType ListingType { get; set; }
        public decimal Price { get; set; }
    }
}
