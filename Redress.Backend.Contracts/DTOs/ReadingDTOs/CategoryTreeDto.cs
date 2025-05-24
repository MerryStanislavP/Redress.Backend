using Redress.Backend.Contracts.DTOs.Enums;
using System.Collections.Generic;

namespace Redress.Backend.Contracts.DTOs.ReadingDTOs
{
    public class CategoryTreeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public Guid? ParentId { get; set; }
        public List<CategoryTreeDto> Children { get; set; } = new List<CategoryTreeDto>();
    }
} 