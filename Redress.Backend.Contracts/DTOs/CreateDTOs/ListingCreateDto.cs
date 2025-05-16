

namespace Redress.Backend.Contracts.DTOs.CreateDTO
{
    public class ListingCreateDto 
    {
        public string Title { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
    }
}
