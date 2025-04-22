using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RedressAPI.Models
{
    public class ListingImage
    {
        [Key]
        public Guid ImageId { get; set; }

        [Required, ForeignKey("Listing")]
        public Guid ListingId { get; set; }

        [Required]
        public string FileName { get; set; } = null!;

        [Required]
        public string Url { get; set; } = null!;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
