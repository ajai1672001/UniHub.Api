using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;
using UniHub.Domain.Entities;
using UniHub.Domain.Entities.Identity;
using UniHub.Domain.Interface;

namespace UniHub.Infrastructure.Configurations;

public static class BaseConfiguration
{
    public static void ConfigureAuditEntity(EntityTypeBuilder builder)
    {
        builder.Property(nameof(IHaveBaseAuditEntityService.DateCreated))
               .IsRequired();

        builder.Property(nameof(IHaveBaseAuditEntityService.CreatedBy))
               .IsRequired(false);

        builder.Property(nameof(IHaveBaseAuditEntityService.DateModified))
               .IsRequired(false);

        builder.Property(nameof(IHaveBaseAuditEntityService.UpdatedBy))
               .IsRequired(false);
    }

    public static void ConfigureBaseEntity(EntityTypeBuilder builder)
    {
        builder.Property<byte[]>(nameof(IHaveBaseEntitySerivce.RowVersion))
               .IsConcurrencyToken()
               .IsRowVersion()
               .ValueGeneratedOnAddOrUpdate();
    }

    public static void ConfigureSoftDeleteAudit(EntityTypeBuilder builder)
    {
        builder.Property(nameof(IHaveBaseSoftDeleteService.IsDeleted))
               .HasDefaultValue(false)
               .IsRequired();
    }

    public static void ConfigureTenantSoftDeleteIdAuditEntity(EntityTypeBuilder builder)
    {
        builder.HasOne(typeof(Tenant), nameof(IHaveBaseTenantSoftDeleteIdAuditEntityService.Tenant))
               .WithMany()
               .HasForeignKey(nameof(IHaveBaseTenantSoftDeleteIdAuditEntityService.TenantId))
               .OnDelete(DeleteBehavior.NoAction);
    }

    public static void ConfigureTenantUserEntity(EntityTypeBuilder builder)
    {
        builder.HasOne(typeof(TenantUser), nameof(IHaveTenantUserIdEntityService.TenantUser))
               .WithMany()
               .HasForeignKey(nameof(IHaveTenantUserIdEntityService.TenantUserId))
               .OnDelete(DeleteBehavior.NoAction);
    }

    public static void ConfigureUserIdEntity(EntityTypeBuilder builder)
    {
        builder.HasOne(typeof(AspNetUser), nameof(IHaveUserIdEntityService.AspNetUser))
               .WithMany()
               .HasForeignKey(nameof(IHaveUserIdEntityService.AspNetUserId))
               .OnDelete(DeleteBehavior.NoAction);
    }
}