using UniHub.Api.MinimalApiEndpoints;

namespace UniHub.Api
{
    public static class MinimalApiRegistration
    {
        public static void RegisterMinimalApis(this WebApplication app)
        {
            // Call each endpoint group here
            app.MapUtilityEndpoints();
        }
    }
}
