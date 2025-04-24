
namespace Redress.Backend.Domain.Entities
{
    public class Auction // аукціон
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? EndAt { get; set; }
        public decimal StartPrice { get; set; }
        public decimal MinStep { get; set; }
        public Guid ListingId { get; set; }
        public Listing? Listing { get; set; }

        public List<Bid> Bids { get; set; } = new List<Bid>();
    }

}
