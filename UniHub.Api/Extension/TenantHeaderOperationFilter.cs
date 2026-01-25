using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using UniHub.Core;

namespace UniHub.Api.Extension;

public class TenantHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Parameters.Add(new OpenApiParameter
        {
            Name = KnownString.Headers.Tenant,
            In = ParameterLocation.Header,
            Required = false, // Set to true if required
            Schema = new OpenApiSchema { Type = JsonSchemaType.String, Format = "uuid" }
        });
    }
}