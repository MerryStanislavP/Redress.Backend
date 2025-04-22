using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace RedressAPI.Models
{
    public class Category
    {
        public Guid CategoryId { get; set; } // категоріяID
        public string Name { get; set; } = string.Empty; // Назва категорії (наприклад: "аксесуари")

        [Column(TypeName = "varchar(20)")]
        public GenderType Gender { get; set; }
        public Guid? ParentCategoryId { get; set; } // для підкатегорій (FK до самої себе)

        [ForeignKey("ParentCategoryId")]
        public Category? ParentCategory { get; set; } //стоило бы протестить
        public ICollection<Category>? SubCategories { get; set; } = new List<Category>(); // для зв'язку з підкатегоріями
    }
    public enum GenderType
    {
        Male,    // Чоловічий
        Female,  // Жіночий
        Kids     // Дитячий
    }
}
