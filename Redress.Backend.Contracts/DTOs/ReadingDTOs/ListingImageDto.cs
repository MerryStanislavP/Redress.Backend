

namespace Redress.Backend.Contracts.DTOs.ReadingDTOs
{
    public class ListingImageDto 
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
