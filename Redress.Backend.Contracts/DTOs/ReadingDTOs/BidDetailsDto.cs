using Redress.Backend.Contracts.DTOs.ReadingDTOs;

namespace Redress.Backend.Contracts.DTOs.ReadingDTOs
{
    public class BidDetailsDto : BidDto
    {
        public DateTime CreatedAt { get; set; }
        public ProfileDto Profile { get; set; }
        public AuctionDto Auction { get; set; }
    }
} 