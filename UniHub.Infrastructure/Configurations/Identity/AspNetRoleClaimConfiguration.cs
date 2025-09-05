using UniHub.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static UniHub.Core.KnownString;

namespace UniHub.Infrastruture.Configurations
{
    public class AspNetRoleClaimConfiguration : IEntityTypeConfiguration<AspNetRoleClaim>
    {
        public void Configure(EntityTypeBuilder<AspNetRoleClaim> builder)
        {
            builder.ToTable("AspNetRoleClaims", Schema.Identity);

            builder
                .Property(x => x.IsDeleted)
                .IsRequired();

            builder
                .HasQueryFilter(e => !e.IsDeleted);
        }
    }
}