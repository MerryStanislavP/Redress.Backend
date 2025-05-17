using Redress.Backend.Contracts.DTOs.Enums;

namespace Redress.Backend.Contracts.DTOs.CreateDTOs
{
    public class CategoryCreateDto 
    {
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public Guid? ParentId { get; set; }
    }
}
