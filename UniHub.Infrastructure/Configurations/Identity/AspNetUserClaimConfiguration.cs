using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniHub.Entities;
using static UniHub.Core.KnownString;

namespace UniHub.Infrastruture.Configurations
{
    public class AspNetUserClaimConfiguration : IEntityTypeConfiguration<AspNetUserClaim>
    {
        public void Configure(EntityTypeBuilder<AspNetUserClaim> builder)
        {
            builder.ToTable("AspNetUserClaims", Schema.Identity);

            builder
                .Property(x => x.IsDeleted)
                .IsRequired();

            builder
                .HasQueryFilter(e => !e.IsDeleted);
        }
    }
}