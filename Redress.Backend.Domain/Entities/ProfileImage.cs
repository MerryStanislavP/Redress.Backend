
namespace Redress.Backend.Domain.Entities
{
    public class ProfileImage //  зображення для профілю
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid ProfileId { get; set; }
        public Profile? Profile { get; set; }
    }
}
