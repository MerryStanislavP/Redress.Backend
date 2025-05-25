using Redress.Backend.Contracts.DTOs.Enums;

namespace Redress.Backend.Contracts.DTOs.ReadingDTOs
{
    public class ProfileDto 
    {
        public Guid Id { get; set; }
        public string? Bio { get; set; }
        public double? AverageRating { get; set; }
        public RatingStatus RatingStatus { get; set; }
    }

}
