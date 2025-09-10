using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using UniHub.Api.Extension;
using UniHub.Api.Extension.Middleware;
using UniHub.Api.Extenstion;
using UniHub.Core;
using UniHub.Infrastructure;
using UniHub.Service;

namespace UniHub.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Serilog

        SerilogConfig.LogConfig();

        #endregion Serilog

        #region Database Connection (Configure EF Core DbContext)

        // Registers ApplicationDbContext with SQL Server connection string.
        // Enables Entity Framework Core to access the database.
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        #endregion Database Connection (Configure EF Core DbContext)

        #region API Explorer & Swagger (For OpenAPI Documentation) & x-apikey  & x-tenant-Id

        // Adds API Explorer for Minimal APIs and Controllers.
        // Adds Swagger/OpenAPI generation for testing and documentation.
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Description = "API Key needed to access the endpoints. Use: `x-api-key: {your_key}`",
                Type = SecuritySchemeType.ApiKey,
                Name = KnownString.Headers.Apikey,        // 👈 header name
                In = ParameterLocation.Header,
                Scheme = "ApiKeyScheme"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            options.OperationFilter<TenantHeaderOperationFilter>();
        });

        #endregion API Explorer & Swagger (For OpenAPI Documentation)

        #region Controllers (Traditional API Endpoints)

        // Registers support for MVC Controllers.
        // This allows attribute-based controllers to work (e.g., [ApiController]).
        builder.Services.AddControllers();

        #endregion Controllers (Traditional API Endpoints)

        #region Dependency Injection (Custom Application Services)

        // Extension method to register application-level services
        // Example: repositories, domain services, business logic handlers.
        builder.Services.AddServices();

        #endregion Dependency Injection (Custom Application Services)

        builder.Host.UseSerilog();

        var app = builder.Build();

        #region GetStaticValues


        app.UseStaticValues();

        #endregion

        #region Swagger Middleware (Development Only)

        // Enables Swagger UI only in Development mode.
        // Provides a web-based interface for API testing & documentation.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "UniHub API V1");
                c.RoutePrefix = "swagger"; // API docs available at /swagger/index.html
            });
        }

        #endregion Swagger Middleware (Development Only)

        #region Middleware Pipeline

        // Error handling
        app.UseMiddleware<ErrorHandlingMiddleware>();

        // X-Apikey authentication
        app.UseMiddleware<ApiKeyMiddleware>();

        // Enforces HTTPS redirection for secure requests.
        app.UseHttpsRedirection();

        // Adds Authorization middleware (for role/claim-based security).
        app.UseAuthorization();

        // Maps controllers to their routes.
        app.MapControllers();

        #endregion Middleware Pipeline

        #region Minimal API Endpoints

        // Extension method that registers all Minimal API endpoints.
        // Keeps endpoint registration modular and clean.
        app.RegisterMinimalApis();

        #endregion Minimal API Endpoints

        app.Run();
    }
}