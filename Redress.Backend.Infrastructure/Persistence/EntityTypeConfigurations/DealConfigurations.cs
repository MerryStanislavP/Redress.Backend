using Redress.Backend.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Redress.Backend.Domain.Entities;

namespace Redress.Backend.Infrastructure.Persistence.EntityTypeConfigurations
{
    public class DealConfigurations : IEntityTypeConfiguration<Deal>
    {
        public void Configure(EntityTypeBuilder<Deal> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Price)
                   .IsRequired()
                   .HasPrecision(18, 2);

            builder.Property(d => d.CreatedAt)
                   .IsRequired();

            builder.Property(d => d.Status)
                   .IsRequired();

            builder.Property(d => d.ListingType)
                   .IsRequired();

            builder.HasOne(d => d.Listing)
                   .WithMany()
                   .HasForeignKey(d => d.ListingId)
                   .IsRequired();

            builder.HasOne(d => d.BuyerProfile)
                   .WithMany()
                   .HasForeignKey(d => d.BuyerProfileId)
                   .IsRequired();
        }
    }
}
