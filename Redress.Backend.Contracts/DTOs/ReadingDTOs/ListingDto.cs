

namespace Redress.Backend.Contracts.DTOs.ReadingDTOs
{
    public class ListingDto 
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public bool IsAuction { get; set; }
    }
}
