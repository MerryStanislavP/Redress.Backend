using Redress.Backend.Contracts.DTOs.Enums;

namespace Redress.Backend.Contracts.DTOs.ReadingDTO
{
    public class CategoryDto 
    {
        public Guid Id { get; set; }
        public Sex Sex { get; set; }
        public Guid? ParentId { get; set; }
    }
}
