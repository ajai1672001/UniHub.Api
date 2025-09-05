using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UniHub.Entities;

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
    }
    private void ApplyConfigurations(ModelBuilder modelBuilder)
    {
        var configurations = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.Namespace == "UniHub.Infrastructure.Configuration" &&
                        t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)));

        foreach (var configuration in configurations)
        {
            dynamic instance = Activator.CreateInstance(configuration);
            modelBuilder.ApplyConfiguration(instance);
        }
    }
    // ✅ Only keep custom tables
    public DbSet<Tenant> Tenants { get; set; }
}