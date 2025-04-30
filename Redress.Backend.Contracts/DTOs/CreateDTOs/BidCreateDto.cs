

namespace Redress.Backend.Contracts.DTOs.CreateDTO
{
    public class BidCreateDto 
    {
        public decimal Amount { get; set; }
        public Guid AuctionId { get; set; }
        public Guid ProfileId { get; set; }
    }
}
