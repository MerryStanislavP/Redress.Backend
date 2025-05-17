using Redress.Backend.Contracts.DTOs.CreateDTOs;

namespace Redress.Backend.Contracts.DTOs.ReadingDTOs
{
    public class AuctionDetailsDto : AuctionDto
    {
        public ListingDto Listing { get; set; }
        public List<BidDto> Bids { get; set; }
        public decimal CurrentPrice { get; set; }
        public int BidCount { get; set; }
        public bool IsActive { get; set; }
    }
} 