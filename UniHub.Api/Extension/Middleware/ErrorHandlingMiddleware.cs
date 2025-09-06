using Microsoft.EntityFrameworkCore;
using System.Net;
using UniHub.Dto;

namespace UniHub.Api.Extension.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly bool _showDetailError;

        public ErrorHandlingMiddleware(RequestDelegate next,
            ILogger<ErrorHandlingMiddleware> logger,
            IConfiguration configuration)
        {
            _next = next;
            _logger = logger;

            // optionally make it configurable from appsettings.json
            _showDetailError = configuration.GetValue<bool>("AppSettings:ShowDetailError", false);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            // --- custom known exceptions ---
            //catch (NoDefaultTenantException ex)
            //{
            //    await HandleExceptionAsync(context, ex, HttpStatusCode.UnprocessableEntity,
            //        ex.Message, ex.Tenants);
            //}
            catch (ApplicationException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.UnprocessableEntity, ex.Message);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.Conflict,
                    _showDetailError ? ex.Message :
                    "No data was updated because the information might have changed or been removed since it was last loaded. Please refresh and try again.");
            }
            catch (UnauthorizedAccessException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.Unauthorized,
                    _showDetailError ? ex.Message : "Unauthorized request");
            }
            // --- fallback unknown exceptions ---
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError,
                    _showDetailError ? ex.Message :
                    "An unhandled exception occurred while processing the request.");
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex,
    HttpStatusCode code, string message = "", dynamic? data = null)
        {
            // 🔹 log to Serilog
            Serilog.Log.ForContext("LogType", "Error")
                       .Error(ex, "Unhandled exception caught by middleware. " + ex.Message);

            _logger.LogError(ex, message);

            if (!context.Response.HasStarted)
            {
                context.Response.Clear(); // clear headers + body if something was written
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.OK;
            }

            var errorResponse = new BaseResponse<dynamic>
            {
                Code = code,
                Message = message,
                Data = data,
                IsSuccess = false
            };

            await context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}