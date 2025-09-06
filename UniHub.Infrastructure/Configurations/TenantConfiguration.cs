using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniHub.Domain.Entities;
using static UniHub.Core.KnownString;

namespace UniHub.Infrastructure.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.ToTable("Tenants", Schema.Tenant);

            builder
                .Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired()
                .IsUnicode(true);

            builder
                .Property(e => e.TimeZone)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}