using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RedressAPI.Models
{
    public class Profile
    {
        //public Guid ProfileId { get; set; } // профільID -> лишнее т.к, оказалось в EF framework одна из таблиц связи один-к-одному должна иметь один и тот же ключ
        //для внутренего и внешнего ключа как внутрений ключ второй таблицы

        [Key, ForeignKey("User")] // PK = FK на UserId
        public Guid UserId { get; set; } // КористувачID (FK)

        public virtual User User { get; set; } = null!;
        public decimal Balance { get; set; } = 0; // баланс профіля

        [MaxLength(500)]
        public string? About { get; set; } // інформація про себе

        [ForeignKey("ProfileImage")]
        public Guid? ProfileImageId { get; set; } // FK від Зображення для профілю

        public virtual ProfileImage ProfileImage { get; set; } = null!; // Один-к-одному

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public int ReviewCount { get; set; } = 0; // кількість оцінок (відгуків)
        public double Rating { get; set; } = 0; // середнє значення рейтингу

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // дата створення
    }
}
