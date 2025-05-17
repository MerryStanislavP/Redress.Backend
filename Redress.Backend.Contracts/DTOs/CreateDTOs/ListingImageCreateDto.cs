

namespace Redress.Backend.Contracts.DTOs.CreateDTOs
{
    public class ListingImageCreateDto 
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public Guid ListingId { get; set; }
    }
}
