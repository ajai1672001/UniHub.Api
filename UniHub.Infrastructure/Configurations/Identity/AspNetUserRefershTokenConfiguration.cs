using UniHub.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static UniHub.Core.KnownString;

namespace UniHub.Infrastruture.Configurations
{
    public class AspNetUserRefershTokenConfiguration : IEntityTypeConfiguration<AspNetUserRefershToken>
    {
        public void Configure(EntityTypeBuilder<AspNetUserRefershToken> builder)
        {

            builder.ToTable("AppUserRefershTokens", Schema.Identity);

            builder.HasKey(x => new { x.UserId, x.Id })
                 .HasName($"PK_AppUserRefershTokens");

            builder
                .Property(x => x.AccessToken)
                .IsRequired();

            builder
                .Property(x => x.RefershToken)
                .IsRequired();

            builder
                .Property(x => x.RefershTokenExpires)
                .IsRequired();

            builder
                .Property(x => x.GenerateAt)
                .IsRequired();
        }
    }
}