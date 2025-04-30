

namespace Redress.Backend.Contracts.DTOs.CreateDTO
{
    public class FeedbackCreateDto 
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        public Guid DealId { get; set; }

    }
}
