using Redress.Backend.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Redress.Backend.Domain.Entities;

namespace Redress.Backend.Infrastructure.Persistence.EntityTypeConfigurations
{
    public class BidConfigurations : IEntityTypeConfiguration<Bid>
    {
        public void Configure(EntityTypeBuilder<Bid> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Amount)
                   .IsRequired()
                   .HasPrecision(18, 2);

            builder.Property(b => b.CreatedAt)
                   .IsRequired();

            builder.HasOne(b => b.Auction)
                   .WithMany()
                   .HasForeignKey(b => b.AuctionId)
                   .IsRequired();

            builder.HasOne(b => b.Profile)
                   .WithMany()
                   .HasForeignKey(b => b.ProfileId)
                   .IsRequired();
            builder.HasOne(b => b.Auction)
                    .WithMany(a => a.Bids)
                    .HasForeignKey(b => b.AuctionId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Profile)
                    .WithMany(p => p.Bids)
                    .HasForeignKey(b => b.ProfileId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);                 // Профиль нельзя удалить если есть ставки
        }
    }
}

