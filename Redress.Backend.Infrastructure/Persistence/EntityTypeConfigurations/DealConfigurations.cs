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

            builder.Property(d => d.Status).IsRequired();
            builder.Property(d => d.ListingType).IsRequired();
            builder.Property(d => d.Price).IsRequired().HasPrecision(18, 2);
            builder.Property(d => d.CreatedAt).IsRequired();

            builder.HasOne(d => d.Listing)
             .WithOne(l => l.Deal)
             .HasForeignKey<Deal>(d => d.ListingId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Cascade);                   // удаление listing удаляет deal

            builder.HasOne(d => d.Profile)
             .WithMany(p => p.Deals)
             .HasForeignKey(d => d.ProfileId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Restrict);                 // нельзя удалить профиль с deals

            builder.HasOne(d => d.Feedback)
             .WithOne(f => f.Deal)
             .HasForeignKey<Feedback>(f => f.DealId)
             .OnDelete(DeleteBehavior.Cascade);                   // удаление deal удаляет feedback
        }
    }
}
