using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;
using UniHub.Domain.Entities;
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
        builder.Property(nameof(IHaveBaseSoftDeleteAuditService.IsDeleted))
               .HasDefaultValue(false)
               .IsRequired();
    }
    public static void ConfigureTenantSoftDeleteIdAuditEntity(EntityTypeBuilder builder, ParameterExpression parameter)
    {
        var optionalTenantIdProperty = Expression
            .Property(parameter, nameof(IHaveBaseTenantSoftDeleteIdAuditEntityService.TenantId));

        builder.HasOne(typeof(Tenant), nameof(IHaveBaseTenantSoftDeleteIdAuditEntityService.Tenant))
               .WithMany()
               .HasForeignKey(nameof(IHaveBaseTenantSoftDeleteIdAuditEntityService.TenantId))
               .OnDelete(DeleteBehavior.NoAction);
    }
}