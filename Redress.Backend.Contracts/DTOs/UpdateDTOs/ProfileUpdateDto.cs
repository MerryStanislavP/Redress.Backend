using Redress.Backend.Contracts.DTOs.Enums;

namespace Redress.Backend.Contracts.DTOs.UpdateDTOs
{
    public class ProfileUpdateDto
    {
        public decimal? Balance { get; set; }
        public string? Bio { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int? RatingCount { get; set; }
        public RatingStatus RatingStatus { get; set; } 
        public double? AverageRating { get; set; }

    }
}
