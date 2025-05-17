using Redress.Backend.Contracts.DTOs.Enums;

namespace Redress.Backend.Contracts.DTOs.ReadingDTOs
{
    public class DealDetailsDto
    {
        public Guid Id { get; set; }
        public DealStatus Status { get; set; }
        public DealType ListingType { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid ListingId { get; set; }
        public Guid ProfileId { get; set; }
        public FeedbackDto? Feedback { get; set; }
        public ListingDto Listing { get; set; }
        public ProfileDto Profile { get; set; }
    }
} 