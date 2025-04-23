using Redress.Backend.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Redress.Backend.Domain.Entities;

namespace Redress.Backend.Infrastructure.Persistence.EntityTypeConfigurations
{
    public class ProfileImageConfigurations : IEntityTypeConfiguration<ProfileImage>
    {
        public void Configure(EntityTypeBuilder<ProfileImage> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(p => p.Url)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(p => p.CreatedAt)
                   .IsRequired();

            builder.HasOne(p => p.Profile)
                   .WithMany()
                   .HasForeignKey(p => p.ProfileId)
                   .IsRequired();
        }
    }
}
