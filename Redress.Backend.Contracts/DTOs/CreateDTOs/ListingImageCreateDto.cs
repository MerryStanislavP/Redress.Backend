

namespace Redress.Backend.Contracts.DTOs.CreateDTO
{
    public class ListingImageCreateDto 
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public Guid ListingId { get; set; }
    }
}
