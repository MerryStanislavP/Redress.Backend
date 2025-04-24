
namespace Redress.Backend.Domain.Entities
{
    public class Favorite //  обране
    {
        public Guid Id { get; set; }
        public Guid ProfileId { get; set; }
        public Profile? Profile { get; set; }
        public Guid ListingId { get; set; }
        public Listing? Listing { get; set; }
    }
}
