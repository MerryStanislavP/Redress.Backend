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
            builder.HasKey(profile => profile.Id);

        }
    }
}
