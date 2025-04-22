using Redress.Backend.Domain.Enums;

namespace Redress.Backend.Domain.Entities
{
    public class Listing // оголошення
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public string Location { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public ListingStatus Status { get; set; }
        public string Description { get; set; }
        public Guid ProfileId { get; set; }
        public Profile Profile { get; set; }
    }
}
