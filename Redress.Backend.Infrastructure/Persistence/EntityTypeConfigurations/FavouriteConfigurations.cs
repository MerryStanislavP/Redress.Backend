using Redress.Backend.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Redress.Backend.Domain.Entities;

namespace Redress.Backend.Infrastructure.Persistence.EntityTypeConfigurations
{
    public class FavouriteConfigurations : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.HasKey(f => f.Id);

            builder.HasOne(f => f.Profile)
                   .WithMany()
                   .HasForeignKey(f => f.ProfileId)
                   .IsRequired();

            builder.HasOne(f => f.Listing)
                   .WithMany()
                   .HasForeignKey(f => f.ListingId)
                   .IsRequired();
        }
    }
}
