using Redress.Backend.Contracts.DTOs.Enums;

namespace Redress.Backend.Contracts.DTOs.CreateDTO
{
    public class CategoryCreateDto 
    {
        public Sex Sex { get; set; }
        public Guid? ParentId { get; set; }
    }
}
