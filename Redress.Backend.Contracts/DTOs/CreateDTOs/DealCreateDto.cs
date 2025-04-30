using Redress.Backend.Contracts.DTOs.Enums;

namespace Redress.Backend.Contracts.DTOs.CreateDTO
{
    public class DealCreateDto 
    {
        public DealStatus Status { get; set; }
        public DealType ListingType { get; set; }
        public decimal Price { get; set; }
        public Guid ListingId { get; set; }
        public Guid ProfileId { get; set; }
    }
}
