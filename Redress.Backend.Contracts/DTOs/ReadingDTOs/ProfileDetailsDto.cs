using Redress.Backend.Contracts.DTOs.Enums;

namespace Redress.Backend.Contracts.DTOs.ReadingDTOs
{
    public class ProfileDetailsDto
    {
        public Guid Id { get; set; }
        public decimal? Balance { get; set; }
        public string? Bio { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? RatingCount { get; set; }
        public RatingStatus RatingStatus { get; set; }
        public double? AverageRating { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public ProfileImageDto? ProfileImage { get; set; }
    }
} 