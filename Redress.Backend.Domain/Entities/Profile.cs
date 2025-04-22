
namespace Redress.Backend.Domain.Entities
{
    public class Profile // профіль
    {
        public Guid Id { get; set; }
        public decimal? Balance { get; set; }
        public string? Bio { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int RatingCount { get; set; }
        public string? RatingStatus { get; set; } // Peremoga
        public double AverageRating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
