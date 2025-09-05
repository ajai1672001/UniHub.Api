using UniHub.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static UniHub.Core.KnownString;

namespace UniHub.Infrastruture.Configurations
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
        }
    }
}