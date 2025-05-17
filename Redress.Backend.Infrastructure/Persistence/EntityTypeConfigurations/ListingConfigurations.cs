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

            builder.Property(l => l.Title).IsRequired().HasMaxLength(100);
            builder.Property(l => l.Latitude);
            builder.Property(l => l.Longitude);
            builder.Property(l => l.Price).IsRequired().HasPrecision(18, 2);
            builder.Property(l => l.CreatedAt).IsRequired();
            builder.Property(l => l.Status).IsRequired();
            builder.Property(l => l.IsAuction).IsRequired();
            builder.Property(l => l.Description).IsRequired().HasMaxLength(1000);

            builder.HasOne(l => l.Profile)
             .WithMany(p => p.Listings)
             .HasForeignKey(l => l.ProfileId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Restrict);                 // если у пользователя есть объявления, удалить профиль нельзя; защищаем данные — объявления должны остаться, пока их явно не удалят


            builder.HasOne(l => l.Category)
             .WithMany(c => c.Listings)
             .HasForeignKey(l => l.CategoryId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(l => l.Auction).WithOne(a => a.Listing).HasForeignKey<Auction>(a => a.ListingId);
            builder.HasOne(l => l.Deal).WithOne(d => d.Listing).HasForeignKey<Deal>(d => d.ListingId);

            builder.HasMany(l => l.Favorites)
             .WithOne(f => f.Listing)
             .HasForeignKey(f => f.ListingId)
             .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(l => l.ListingImages)
             .WithOne(img => img.Listing)
             .HasForeignKey(img => img.ListingId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
