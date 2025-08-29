namespace UniHub.Api.MinimalApiEndpoints
{
    public static class UtilityEndpoints
    {
        public static void MapUtilityEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/ping", () => Results.Ok("Pong ✅"))
               .WithName("Ping")
               .WithTags("Utility");

            app.MapGet("/time", () =>
            {
                return Results.Ok(new { ServerTime = DateTime.UtcNow });
            })
            .WithName("ServerTime")
            .WithTags("Utility");
        }
    }
}
