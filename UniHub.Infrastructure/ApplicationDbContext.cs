using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using UniHub.Domain.Entities;
using UniHub.Domain.Entities.Identity;
using UniHub.Domain.Interface;
using UniHub.Infrastructure.Configurations;

namespace UniHub.Infrastructure;

public class ApplicationDbContext : IdentityDbContext<
        AspNetUser,
        AspNetRole,
        Guid,
        AspNetUserClaim,
        AspNetUserRole,
        AspNetUserLogin,
        AspNetRoleClaim,
        AspNetUserToken>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly IHeaderProvider _headerService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IHttpContextAccessor httpContextAccessor,
        IHeaderProvider headerService)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
        _headerService = headerService;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        ApplyConfigurations(modelBuilder);
        ConfigureEntities(modelBuilder);
    }

    private void ApplyConfigurations(ModelBuilder modelBuilder)
    {
        var configurations = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.Namespace == "UniHub.Infrastructure.Configurations" &&
                        t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)));

        foreach (var configuration in configurations)
        {
            dynamic instance = Activator.CreateInstance(configuration);
            modelBuilder.ApplyConfiguration(instance);
        }
    }

    private void ApplyTenantFilter(ModelBuilder modelBuilder)
    {
        var tenantId = _headerService.TenantId;

        modelBuilder.Entity<Tenant>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<TenantUser>().HasQueryFilter(e => e.TenantId == tenantId && !e.IsDeleted);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var tenantId = _headerService.TenantId;

        foreach (var item in ChangeTracker.Entries().Where(e => e.Entity is IHaveBaseEntitySerivce))
        {
            if (item.State == EntityState.Added)
            {

                if (item.Entity != null)
                {
                    var tenantProperty = item.Entity.GetType().GetProperty("TenantId");
                    if (tenantProperty != null)
                    {
                        var currentValue = (Guid)tenantProperty.GetValue(item.Entity);
                        if (currentValue == Guid.Empty && tenantId != Guid.Empty)
                        {
                            tenantProperty.SetValue(item.Entity, tenantId);
                        }
                    }
                }
                if (tenantId != Guid.Empty)
                {
                    if (item.Entity is IHaveBaseTenantSoftDeleteIdAuditEntityService baseOptionalTenant)
                    {
                        baseOptionalTenant.TenantId = tenantId;
                    }
                }

                if (item.Entity is BaseSoftDeleteIdEntity<Guid> baseId)
                {
                    if (baseId.Id == Guid.Empty)
                    {
                        baseId.Id = Guid.NewGuid();
                    }
                }

                if (item.Entity is IHaveBaseAuditEntityService baseAudit)
                {
                    baseAudit.DateCreated = DateTime.UtcNow;
                }
            }
            else if (item.State == EntityState.Modified)
            {
                if (item.Entity is IHaveBaseAuditEntityService baseAudit)
                {
                    baseAudit.DateModified = DateTime.UtcNow;
                }
            }
        }
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private void ConfigureEntities(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;
            var builder = modelBuilder.Entity(clrType);
            var parameter = Expression.Parameter(clrType, "e");

            if (typeof(IHaveBaseAuditEntityService).IsAssignableFrom(clrType))
            {
                BaseConfiguration.ConfigureAuditEntity(builder);
            }

            if (typeof(IHaveBaseEntitySerivce).IsAssignableFrom(clrType))
            {
                BaseConfiguration.ConfigureBaseEntity(builder);
            }

            if (typeof(IHaveBaseSoftDeleteService).IsAssignableFrom(clrType))
            {
                BaseConfiguration.ConfigureSoftDeleteAudit(builder, entityType, parameter);
            }

            if (typeof(IHaveUserIdEntityService).IsAssignableFrom(clrType))
            {
                BaseConfiguration.ConfigureUserIdEntity(builder);
            }

            if (typeof(IHaveBaseTenantSoftDeleteIdAuditEntityService).IsAssignableFrom(clrType))
            {
                BaseConfiguration.ConfigureTenantSoftDeleteIdAuditEntity(builder);
            }

            if (typeof(IHaveTenantUserIdEntityService).IsAssignableFrom(clrType))
            {
                BaseConfiguration.ConfigureTenantUserEntity(builder);
            }
        }
    }

    // ✅ Only keep custom tables
    public DbSet<Tenant> Tenants { get; set; }

    public DbSet<TenantUser> TenantUsers { get; set; }
    public DbSet<AspNetRole> AspNetRoles { get; set; }
    public DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
    public DbSet<AspNetUser> AspNetUsers { get; set; }
    public DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
    public DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
    public DbSet<AspNetUserRefershToken> AspNetUserRefershTokens { get; set; }
    public DbSet<AspNetUserRole> AspNetUserRole { get; set; }
    public DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
    public DbSet<UserOtp> UserOtps { get; set; }
}