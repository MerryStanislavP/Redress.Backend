
namespace Redress.Backend.Domain.Entities
{
    public class Feedback // відгук
    {
        public Guid Id { get; set; }
        public Guid DealId { get; set; }
        public Deal Deal { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
