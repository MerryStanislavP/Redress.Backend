namespace Redress.Backend.Contracts.DTOs.CreateDTOs
{
    public class FeedbackCreateDto 
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        public Guid DealId { get; set; }
    }
}
