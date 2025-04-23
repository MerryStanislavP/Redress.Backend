using Redress.Backend.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Redress.Backend.Domain.Entities;

namespace Redress.Backend.Infrastructure.Persistence.EntityTypeConfigurations
{
    public class AuctionConfigurations : IEntityTypeConfiguration<Auction>
    {
        public void Configure(EntityTypeBuilder<Auction> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.CreatedAt)
                   .IsRequired();

            builder.Property(a => a.EndAt)
                   .IsRequired();

            builder.Property(a => a.StartPrice)
                   .IsRequired()
                   .HasPrecision(18, 2);

            builder.Property(a => a.MinStep)
                   .IsRequired()
                   .HasPrecision(18, 2);

            builder.HasOne(a => a.Listing)
                   .WithOne()
                   .HasForeignKey<Auction>(a => a.ListingId)
                   .IsRequired();
        }
    }
}
