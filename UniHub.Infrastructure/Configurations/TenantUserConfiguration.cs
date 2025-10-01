using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniHub.Core.Enum;
using UniHub.Domain.Entities;
using static UniHub.Core.KnownString;

namespace UniHub.Infrastructure.Configurations
{
    public class TenantUserConfiguration : IEntityTypeConfiguration<TenantUser>
    {
        public void Configure(EntityTypeBuilder<TenantUser> builder)
        {
            builder.ToTable("TenantUsers", Schema.Tenant);

            builder
                .Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(e => e.LastName)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(e => e.Gender)
                .HasDefaultValue(GenderEnum.NotSpecified)
                .IsRequired();

            builder
                .Property(e => e.DateOfBirth)
                .IsRequired();

            builder
                .Property(e => e.TimeZone)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property( e => e.IsPrimary )
                .HasDefaultValue(false)
                .IsRequired();

            builder
                .HasOne(e => e.AspNetRole)
                .WithMany()
                .HasPrincipalKey(e => e.Id)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}