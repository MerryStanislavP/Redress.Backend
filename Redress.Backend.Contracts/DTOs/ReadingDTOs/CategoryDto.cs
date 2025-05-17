using Redress.Backend.Contracts.DTOs.Enums;

namespace Redress.Backend.Contracts.DTOs.ReadingDTOs
{
    public class CategoryDto 
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public Sex Sex { get; set; }
        public Guid? ParentId { get; set; }
    }
}
