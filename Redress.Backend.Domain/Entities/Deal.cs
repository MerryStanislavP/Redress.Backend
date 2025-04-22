using Redress.Backend.Domain.Enums;

namespace Redress.Backend.Domain.Entities
{
    public class Deal // угода
    {
        public Guid Id { get; set; }
        public Guid ListingId { get; set; }
        public Listing Listing { get; set; }
        public Guid BuyerProfileId { get; set; }
        public Profile BuyerProfile { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DealStatus Status { get; set; }
        public DealType ListingType { get; set; } 
    }
}
