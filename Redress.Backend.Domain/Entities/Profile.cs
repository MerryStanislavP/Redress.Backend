
namespace Redress.Backend.Domain.Entities
{
    public class Profile // профіль
    {
        public Guid Id { get; set; }
        public decimal? Balance { get; set; }
        public string? Bio { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public int? RatingCount { get; set; }
        public string? RatingStatus { get; set; } 
        public double? AverageRating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public User? User { get; set; } // навигационное свойство User
        public Guid UserId { get; set; } // FK User, зависимость от сущности User
        public ProfileImage? ProfileImage {  get; set; } // навигационное свойство ProfileImage
        public List<Bid> Bids { get; set; } = new List<Bid>();
        public List<Favorite> Favorites { get; set; } = new List<Favorite>();
        public List<Listing> listings { get; set; } = new List<Listing>();
        public List<Deal> deals { get; set; } = new List<Deal>();
    }
}
