
namespace Redress.Backend.Domain.Entities
{
    public class ListingImage // зобрадення для оголошень
    {
        public Guid Id { get; set; }
        public Guid ListingId { get; set; }
        public Listing Listing { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
