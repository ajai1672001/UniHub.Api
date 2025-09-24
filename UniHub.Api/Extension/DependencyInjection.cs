using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using UniHub.Domain.Entities.Identity;
using UniHub.Infrastructure;

namespace UniHub.Api.Extension;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentiyConfiguration(this IServiceCollection services)
    {
        services.AddIdentity<AspNetUser, AspNetRole>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireLowercase = true;
            options.User.RequireUniqueEmail = true;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
        })
       .AddUserStore<UserStore<AspNetUser, AspNetRole, ApplicationDbContext, Guid, AspNetUserClaim, AspNetUserRole, AspNetUserLogin, AspNetUserToken, AspNetRoleClaim>>()
       .AddRoleStore<RoleStore<AspNetRole, ApplicationDbContext, Guid, AspNetUserRole, AspNetRoleClaim>>()
       .AddUserManager<UserManager<AspNetUser>>()
       .AddRoleManager<RoleManager<AspNetRole>>()
       .AddEntityFrameworkStores<ApplicationDbContext>()
       .AddDefaultTokenProviders();

        return services;
    }

}
