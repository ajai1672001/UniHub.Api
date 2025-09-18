using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UniHub.Domain.Interface;
using UniHub.Dto;
using UniHub.Infrastructure;
using UniHub.Service.Services;
using Mapster;

namespace UniHub.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITimeService, TimeService>();
            services.AddScoped<ISettingService, SettingService>();
            return services;
        }

        public static IApplicationBuilder UseStaticValues(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var tenant = dbContext.Tenants.IgnoreQueryFilters().Where(e => !e.IsDeleted).AsNoTracking().ToList();

            TenantConfigDto.Tenants = tenant.Adapt<IEnumerable<TenantDto>>();

            return app;
        }
    }
}