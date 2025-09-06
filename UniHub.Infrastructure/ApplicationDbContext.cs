using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using UniHub.Domain.Entities;
using UniHub.Domain.Interface;
using UniHub.Infrastructure.Configurations;

namespace UniHub.Infrastructure;

public class ApplicationDbContext : DbContext
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
            .Where(t => t.Namespace == "UniHub.Infrastruture.Configurations" &&
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

            if (typeof(IHaveBaseSoftDeleteAuditService).IsAssignableFrom(clrType))
            {
                BaseConfiguration.ConfigureSoftDeleteAudit(builder);
            }

            if (typeof(IHaveBaseSoftDeleteAuditService).IsAssignableFrom(clrType))
            {
                BaseConfiguration.ConfigureSoftDeleteAudit(builder);
            }
        }
    }

    // ✅ Only keep custom tables
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Sample> Samples { get; set; }
}