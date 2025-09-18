namespace UniHub.Api.Extension.Middleware;

public class ApiKeyMiddleware
{
    private const string APIKEY_HEADER = "x-api-key";
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(APIKEY_HEADER, out var extractedApiKey))
        {
            throw new UnauthorizedAccessException("API Key was not provided.");
        }

        var configuredKey = _configuration.GetValue<string>("AppSettings:ApiKey");

        if (!string.Equals(extractedApiKey, configuredKey))
        {
            throw new UnauthorizedAccessException("API Key was not valid.");
        }

        await _next(context);
    }
}

