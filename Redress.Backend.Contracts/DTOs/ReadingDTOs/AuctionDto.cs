
namespace Redress.Backend.Contracts.DTOs.ReadingDTO
{
    public class AuctionDto 
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? EndAt { get; set; }
        public decimal StartPrice { get; set; }
        public decimal MinStep { get; set; }
    }
}
