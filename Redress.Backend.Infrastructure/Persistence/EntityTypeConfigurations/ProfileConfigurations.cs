using Redress.Backend.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Redress.Backend.Domain.Entities;

namespace Redress.Backend.Infrastructure.Persistence.EntityTypeConfigurations
{
    public class ProfileConfigurations : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Balance)
                   .HasPrecision(18, 2);

            builder.Property(p => p.Bio)
                   .HasMaxLength(1000);

            builder.Property(p => p.Latitude)
                   .IsRequired();

            builder.Property(p => p.Longitude)
                   .IsRequired();

            builder.Property(p => p.RatingCount)
                   .IsRequired();

            builder.Property(p => p.RatingStatus)
                   .HasMaxLength(100);

            builder.Property(p => p.AverageRating)
                   .IsRequired();

            builder.Property(p => p.CreatedAt)
                   .IsRequired();

            builder.HasOne(p => p.User)
                   .WithOne(u => u.Profile)
                   .HasForeignKey<Profile>(p => p.UserId)
                   .IsRequired();
        }
    }
}
