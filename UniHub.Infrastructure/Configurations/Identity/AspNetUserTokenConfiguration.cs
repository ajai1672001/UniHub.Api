using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniHub.Domain.Entities.Identity;
using static UniHub.Core.KnownString;

namespace UniHub.Infrastructure.Configurations
{
    public class AspNetUserTokenConfiguration : IEntityTypeConfiguration<AspNetUserToken>
    {
        void IEntityTypeConfiguration<AspNetUserToken>.Configure(EntityTypeBuilder<AspNetUserToken> builder)
        {
            builder.ToTable("AspNetUserTokens", Schema.Identity);
        }
    }
}