using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace RedressAPI.Models
{
    public class ProfileImage
    {
        [Key]
        public Guid ImageId { get; set; }

        [Required, ForeignKey("Profile")]
        public Guid ProfileId { get; set; }

        [Required]
        public string FileName { get; set; } = null!;

        [Required]
        public string Url { get; set; } = null!;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
