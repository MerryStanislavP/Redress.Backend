

namespace Redress.Backend.Contracts.DTOs.CreateDTOs
{
    public class AuctionCreateDto 
    {
        public DateTime? EndAt { get; set; }
        public decimal StartPrice { get; set; }
        public decimal MinStep { get; set; }
        public Guid ListingId { get; set; }
    }
}
