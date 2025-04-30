using Redress.Backend.Contracts.DTOs.Enums;

namespace Redress.Backend.Contracts.DTOs.ReadingDTO
{
    public class ProfileDto // думаю, этого будет мало, надо будет добавить также CreateProfileDto и UpdateProfileDto, но это на этапе контроллеров уже
    {
        public Guid Id { get; set; }
        public string? Bio { get; set; }
        public double? AverageRating { get; set; }
        public RatingStatus RatingStatus { get; set; }
    }

}
