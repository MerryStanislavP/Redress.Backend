using Redress.Backend.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Redress.Backend.Domain.Entities;

namespace Redress.Backend.Infrastructure.Persistence.EntityTypeConfigurations
{
    public class ListingConfigurations : IEntityTypeConfiguration<Listing>
    {
        public void Configure(EntityTypeBuilder<Listing> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(l => l.Location)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(l => l.Price)
                   .IsRequired()
                   .HasPrecision(18, 2);

            builder.Property(l => l.CreatedAt)
                   .IsRequired();

            builder.Property(l => l.Status)
                   .IsRequired();

            builder.Property(l => l.Description)
                   .IsRequired()
                   .HasMaxLength(3000);

            builder.HasOne(l => l.Category)
                   .WithMany()
                   .HasForeignKey(l => l.CategoryId)
                   .IsRequired();

            builder.HasOne(l => l.Profile)
                   .WithMany()
                   .HasForeignKey(l => l.ProfileId)
                   .IsRequired();
        }
    }
}
