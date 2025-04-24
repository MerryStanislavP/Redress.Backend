
namespace Redress.Backend.Domain.Entities
{
    public class Feedback // відгук
    {
        public Guid Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = "No comments";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid DealId { get; set; } // FK User, зависимость от сущности Deal
        public Deal? Deal { get; set; } // навигационное свойство Deal
    }
}
