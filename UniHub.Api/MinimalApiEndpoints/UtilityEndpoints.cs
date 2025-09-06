using UniHub.Domain.Interface;

namespace UniHub.Api.MinimalApiEndpoints
{
    public static class UtilityEndpoints
    {
        public static void MapUtilityEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/ping", () => Results.Ok("Pong ✅"))
                .WithName("Ping")
                .WithTags("Utility");

            app.MapGet("/time", (ITimeService timeService) =>
                {
                    var time = timeService.GetCurrentTime();
                    return Results.Ok(new { ServerTime = time });
                })
                .WithName("ServerTime")
                .WithTags("Utility");
        }
    }
}