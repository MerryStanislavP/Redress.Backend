using Redress.Backend.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Redress.Backend.Domain.Entities;

namespace Redress.Backend.Infrastructure.Persistence.EntityTypeConfigurations
{
    public class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Sex)
                   .IsRequired();
            builder.Property(c => c.ParentId);

            builder.HasOne(c => c.Parent)
                   .WithMany(p => p.Children)
                   .HasForeignKey(c => c.ParentId)
                   .OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(c => c.Listings)
                    .WithOne(l => l.Category)
                    .HasForeignKey(l => l.CategoryId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);                 // Нельзя удалять использованные категории
        }
    }
}
