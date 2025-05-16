using Redress.Backend.Contracts.DTOs.Enums;

namespace Redress.Backend.Contracts.DTOs.ReadingDTOs
{
    public class ListingDetailsDto
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
        public Guid CategoryId { get; set; }
    }
}
