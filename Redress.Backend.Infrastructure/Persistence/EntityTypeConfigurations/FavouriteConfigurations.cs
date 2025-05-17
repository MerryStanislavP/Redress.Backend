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
            builder.HasIndex(f => new { f.ProfileId, f.ListingId }).IsUnique();

            builder.HasOne(f => f.Profile)
                   .WithMany()
                   .HasForeignKey(f => f.ProfileId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);                   // удаление profile удаляет favs

            builder.HasOne(f => f.Listing)
                    .WithMany(l => l.Favorites)
                    .HasForeignKey(f => f.ListingId)
                    .IsRequired()
                     .OnDelete(DeleteBehavior.Cascade); // удаление listing удаляет favs
        }
    }
}
