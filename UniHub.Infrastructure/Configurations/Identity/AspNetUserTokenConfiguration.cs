using UniHub.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static UniHub.Core.KnownString;

namespace UniHub.Infrastruture.Configurations
{
    public class AspNetUserTokenConfiguration : IEntityTypeConfiguration<AspNetUserToken>
    {
        public void Configure(EntityTypeBuilder<AspNetUserToken> builder)
        {
            builder.ToTable("AspNetUserTokens", Schema.Identity);

            builder
                .Property(x => x.IsDeleted)
                .IsRequired();

            builder
                .HasQueryFilter(e => !e.IsDeleted);
        }
    }
}