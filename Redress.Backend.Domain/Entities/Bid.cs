
namespace Redress.Backend.Domain.Entities
{
    public class Bid // ставка
    {
        public Guid Id { get; set; }
        public Guid AuctionId { get; set; }
        public Auction Auction { get; set; }
        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
