using Redress.Backend.Contracts.DTOs.Enums;

namespace Redress.Backend.Contracts.DTOs.UpdateDTOs
{
    public class DealUpdateDto
    {
        public DealStatus Status { get; set; } 
        public decimal Price { get; set; }
        public Guid ListingId { get; set; }
        public Guid ProfileId { get; set; }

    }
}
