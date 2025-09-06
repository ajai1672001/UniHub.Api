using Microsoft.AspNet.Identity.EntityFramework;
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
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

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
                BaseConfiguration.ConfigureSoftDeleteAudit(builder);
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
    public DbSet<AspNetRoleClaim> AspNetRoleClaims   { get; set; }
    public DbSet<AspNetUser> AspNetUsers { get; set; }
    public DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
    public DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
    public DbSet<AspNetUserRefershToken> AspNetUserRefershTokens { get; set; }
    public DbSet<AspNetUserRole> AspNetUserRole { get; set; }
    public DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
    public DbSet<UserOtp> UserOtps { get; set; }
}