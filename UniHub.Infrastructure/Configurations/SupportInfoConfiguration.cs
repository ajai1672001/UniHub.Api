using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniHub.Domain.Entities;

namespace UniHub.Infrastructure.Configurations
{
    public class SupportInfoConfiguration : IEntityTypeConfiguration<SupportInfo>
    {
        public void Configure(EntityTypeBuilder<SupportInfo> builder)
        {
            builder.ToTable("SupportInfos", "tenant");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.SupportEmail)
                   .HasMaxLength(200);

            builder.Property(x => x.SupportPhone)
                   .HasMaxLength(20);

            builder.Property(x => x.WorkingHours)
                   .HasMaxLength(150);

            builder.Property(x => x.TenantId)
                   .IsRequired();

            builder.Property(x => x.IsDeleted)
                   .HasDefaultValue(false);
        }
    }
}
