using Redress.Backend.Domain.Enums;

namespace Redress.Backend.Domain.Entities
{
    public class Deal // угода
    {
        public Guid Id { get; set; }
        public DealStatus Status { get; set; } = DealStatus.Pending;
        public DealType ListingType { get; set; } = DealType.Sale;
        public decimal Price { get; set; } = decimal.Zero;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Feedback? Feedback { get; set; } // навигационное свойство Feedback
        public Listing? Listing { get; set; } // навигационное свойство Listing
        public Guid ListingId {  get; set; }
        public Profile? Profile { get; set; }
        public Guid ProfileId { get; set; }
    }
}
