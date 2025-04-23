using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redress.Backend.Infrastructure.Persistence.EntityTypeConfigurations
{
    public class FeedbackConfigurations : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Rating)
                   .IsRequired();

            builder.Property(f => f.Comment)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(f => f.CreatedAt)
                   .IsRequired();

            builder.HasOne(f => f.Deal)
                   .WithMany()
                   .HasForeignKey(f => f.DealId)
                   .IsRequired();
        }
    }
}
