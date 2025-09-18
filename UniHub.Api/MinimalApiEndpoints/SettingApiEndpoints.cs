using UniHub.Domain.Interface;

namespace UniHub.Api.MinimalApiEndpoints
{
    public static class SettingApiEndpoints
    {
        public static void MapSettingApiEndpoints(this IEndpointRouteBuilder app)
        {

            app.MapGet("/setting", (ISettingService settingService) =>
            {


                return Results.Ok();
            })
            .WithName("ServerTime")
            .WithTags("Utility");
        }
    }
}
