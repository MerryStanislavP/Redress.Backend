using Redress.Backend.Contracts.DTOs.Enums;

namespace Redress.Backend.Contracts.DTOs.ReadingDTOs
{
    public class FeedbackDetailsDto
    {
        public Guid Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid DealId { get; set; }
        public DealDto Deal { get; set; }
        public ProfileDto Profile { get; set; }
    }
} 