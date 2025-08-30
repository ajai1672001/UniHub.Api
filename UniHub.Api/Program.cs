using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using UniHub.Api.Extenstion;
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
        #endregion

        #region Database Connection (Configure EF Core DbContext)
        // Registers ApplicationDbContext with SQL Server connection string.
        // Enables Entity Framework Core to access the database.
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        #endregion

        #region API Explorer & Swagger (For OpenAPI Documentation)
        // Adds API Explorer for Minimal APIs and Controllers.
        // Adds Swagger/OpenAPI generation for testing and documentation.
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        #endregion

        #region Controllers (Traditional API Endpoints)
        // Registers support for MVC Controllers.
        // This allows attribute-based controllers to work (e.g., [ApiController]).
        builder.Services.AddControllers();
        #endregion

        #region Dependency Injection (Custom Application Services)
        // Extension method to register application-level services
        // Example: repositories, domain services, business logic handlers.
        builder.Services.AddServices();
        #endregion

        builder.Host.UseSerilog();

        var app = builder.Build();

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
        #endregion

        #region Middleware Pipeline
        // Enforces HTTPS redirection for secure requests.
        app.UseHttpsRedirection();

        // Adds Authorization middleware (for role/claim-based security).
        app.UseAuthorization();

        // Maps controllers to their routes.
        app.MapControllers();
        #endregion

        #region Minimal API Endpoints
        // Extension method that registers all Minimal API endpoints.
        // Keeps endpoint registration modular and clean.
        app.RegisterMinimalApis();
        #endregion

        app.Run();
    }
}
