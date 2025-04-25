using Redress.Backend.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Redress.Backend.Domain.Entities;

namespace Redress.Backend.Infrastructure.Persistence.EntityTypeConfigurations
{
    public class ProfileConfigurations : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> bilder)
        {
            bilder.HasKey(p => p.Id);

            bilder.Property(p => p.Balance).HasPrecision(18, 2);
            bilder.Property(p => p.Bio);
            bilder.Property(p => p.Latitude);
            bilder.Property(p => p.Longitude);
            bilder.Property(p => p.RatingCount);
            bilder.Property(p => p.RatingStatus).IsRequired();
            bilder.Property(p => p.AverageRating);
            bilder.Property(p => p.CreatedAt).IsRequired();

            bilder.HasOne(p => p.User)
             .WithOne(u => u.Profile)
             .HasForeignKey<Profile>(p => p.UserId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Cascade);

            bilder.HasOne(p => p.ProfileImage)
             .WithOne(pi => pi.Profile)
             .HasForeignKey<ProfileImage>(pi => pi.ProfileId)
             .OnDelete(DeleteBehavior.Cascade);

            bilder.HasMany(p => p.Bids)
             .WithOne(b => b.Profile)
             .HasForeignKey(b => b.ProfileId)
             .OnDelete(DeleteBehavior.Cascade);

            bilder.HasMany(p => p.Favorites)
             .WithOne(f => f.Profile)
             .HasForeignKey(f => f.ProfileId)
             .OnDelete(DeleteBehavior.Cascade);

            bilder.HasMany(p => p.Listings)
             .WithOne(l => l.Profile)
             .HasForeignKey(l => l.ProfileId)
             .OnDelete(DeleteBehavior.Cascade);

            bilder.HasMany(p => p.Deals)
             .WithOne(d => d.Profile)
             .HasForeignKey(d => d.ProfileId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
