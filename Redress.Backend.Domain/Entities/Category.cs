using Redress.Backend.Domain.Enums;

namespace Redress.Backend.Domain.Entities
{
    public class Category // категорія
    { 
        public Guid Id { get; set; }
        public Sex Sex { get; set; }
        public Guid? ParentId { get; set; }
        public Category? Parent { get; set; }
        public List<Category> Children { get; set; }
    }
}
