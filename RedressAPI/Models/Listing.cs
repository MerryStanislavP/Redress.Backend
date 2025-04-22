using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RedressAPI.Models
{
    public class Listing
    {
        [Key]
        public Guid ListingId { get; set; } // оголошенняID

        [Required]
        public string Title { get; set; } // назва оголошення

        [Required, ForeignKey("Category")]
        public int CategoryId { get; set; } // категорія оголошення (FK)

        public Category Category { get; set; }

        public string? Latitude { get; set; } //широта, координата
        public string? Longitude { get; set; } // долгота

        [Required]
        public decimal Price { get; set; } // ціна товару

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedAt { get; set; } // хз вдоках так написано
        public ListingStatus Status { get; set; } = ListingStatus.Active; // статус оголошення (активно, приховано, продано)
        public string Description { get; set; } // Опис товару

        [Required, ForeignKey("Profile")]
        public Guid ProfileId { get; set; }
        public virtual Profile Profile { get; set; } = null!;

        public List<ListingImage> Images { get; set; }
    }
}
public enum ListingStatus
{
    Active,
    Hidden,
    Sold
}
