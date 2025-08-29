using Microsoft.EntityFrameworkCore;
using UniHub.Infrastructure;
namespace UniHub.Api; 
public class Program 
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Configure Services 
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddEndpointsApiExplorer(); 
        builder.Services.AddSwaggerGen();
        #endregion

        builder.Services.AddControllers();
        var app = builder.Build();

        #region Configure Middleware 
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(); app.UseSwaggerUI(c =>
            {
                // This ensures swagger/index.html is accessible
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "UniHub API V1");
                c.RoutePrefix = "swagger";
                // available at /swagger/index.html
            });
        }
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        #endregion

        #region Minimal API Endpoints
        app.RegisterMinimalApis();
        #endregion

        app.Run();
    } 
}