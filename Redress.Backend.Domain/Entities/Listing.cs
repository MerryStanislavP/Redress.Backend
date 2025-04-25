using Redress.Backend.Domain.Enums;

namespace Redress.Backend.Domain.Entities
{
    public class Listing // оголошення
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ListingStatus Status { get; set; } = ListingStatus.Active;
        public bool IsAuction { get; set; } = false;
        public string Description { get; set; }
        public Guid ProfileId { get; set; }
        public Profile? Profile { get; set; }

        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }

        public Auction? Auction { get; set; }
        public Deal? Deal { get; set; }

        public List<Favorite> Favorites { get; set; } = new List<Favorite>();

        public List<ListingImage> ListingImages { get; set; } = new List<ListingImage>();
    }
}
