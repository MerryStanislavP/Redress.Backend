namespace Redress.Backend.Contracts.DTOs.UpdateDTOs
{
    public class ListingUpdateDto 
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Guid? CategoryId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
