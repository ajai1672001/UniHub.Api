using Microsoft.Extensions.DependencyInjection;
using UniHub.Service.Interfaces;
using UniHub.Service.Services;

namespace UniHub.Service
{
    public static class DependencyInjection
    {
        public static  IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITimeService, TimeService>();
            return services;
        }
    }
}
