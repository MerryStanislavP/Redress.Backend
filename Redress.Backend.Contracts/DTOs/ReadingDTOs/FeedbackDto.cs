

namespace Redress.Backend.Contracts.DTOs.ReadingDTOs
{
    public class FeedbackDto
    {
        public Guid Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
