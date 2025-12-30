using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniHub.Core.Enum;
using UniHub.Domain.Entities;

namespace UniHub.Infrastructure.Configurations
{
    public class SocialLinkConfiguration : IEntityTypeConfiguration<SocialLink>
    {
        public void Configure(EntityTypeBuilder<SocialLink> builder)
        {
            builder.ToTable("SocialLinks", "tenant", table =>
            {
                table.HasCheckConstraint(
                    "CK_SocialLinks_Platform",
                    $"Platform IN ({string.Join(", ", Enum.GetValues<SocialPlatformEnum>().Cast<int>())})"
                );
            });

            builder.HasKey(x => x.Id).IsClustered(false);

            builder.HasIndex(x => new { x.TenantId, x.Id }).IsClustered();

            builder.Property(x => x.Platform)
                   .IsRequired()
                   .HasConversion<int>();

            builder.Property(x => x.Url)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                   .HasDefaultValue(true);

        }
    }
}