using Redress.Backend.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Redress.Backend.Domain.Entities;

namespace Redress.Backend.Infrastructure.Persistence.EntityTypeConfigurations
{
    public class ListingImageConfigurations : IEntityTypeConfiguration<ListingImage>
    {
        public void Configure(EntityTypeBuilder<ListingImage> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Name)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(i => i.Url)
                   .IsRequired();

            builder.Property(i => i.CreatedAt)
                   .IsRequired();

            builder.HasOne(i => i.Listing)
                   .WithMany(l => l.ListingImages)
                   .HasForeignKey(i => i.ListingId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
