using UniHub.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static UniHub.Core.KnownString;

namespace UniHub.Infrastruture.Configurations
{
    public class AspNetRoleConfiguration : IEntityTypeConfiguration<AspNetRole>
    {
        public void Configure(EntityTypeBuilder<AspNetRole> builder)
        {
            builder.ToTable("AspNetRoles", Schema.Identity);

            builder
                .Property(x => x.IsDeleted)
                .IsRequired();

            builder
                .Property(x => x.DisplayName)
                .HasMaxLength(50)
                .IsRequired();

            builder .Property(x => x.CreatedDate)
                .IsRequired();

            builder
                .HasOne<Tenant>()
                .WithMany()
                .HasPrincipalKey(e => e.Id)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasQueryFilter(e => !e.IsDeleted);
        }
    }
}