using Mapster;
using UniHub.Core;
using UniHub.Dto;
using UniHub.Infrastructure;

namespace UniHub.Api.Extension.Middleware;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IHeaderProvider headerService, ApplicationDbContext dbContext)
    {
        var endpoint = context.GetEndpoint();

        if (endpoint?.Metadata.GetMetadata<SkipTenantMiddlewareAttribute>() != null)
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(KnownString.Headers.Tenant, out var extractedTenantIdString))
        {
            throw new ApplicationException("X-Tenant-Id is missing");
        }

        if (!Guid.TryParse(extractedTenantIdString.ToString(), out var tenantId))
        {
            throw new ApplicationException("Invalid Tenant Id format");
        }

        var tenant = TenantConfigDto.Tenants.FirstOrDefault(t => t.Id == tenantId);// soft delete check if you use it

        if (tenant == null)
        {
            throw new ApplicationException("Invalid Tenant Id");
        }

        // Set values to header service
        headerService.TenantId = tenantId;
        headerService.CurrentTenant = tenant.Adapt<TenantDto>();

        await _next(context);
    }

}
