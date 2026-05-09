using System.Net;
using System.Text.Json;
using PetManagementSystem.Api.Exceptions;

namespace PetManagementSystem.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<GlobalExceptionMiddleware>
            _logger;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }

            // ===============================
            // Bad Request Exception
            // ===============================

            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex,
                    "Bad Request Exception");

                await HandleExceptionAsync(
                    context,
                    HttpStatusCode.BadRequest,
                    ex.Message);
            }

            // ===============================
            // Not Found Exception
            // ===============================

            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex,
                    "Resource Not Found");

                await HandleExceptionAsync(
                    context,
                    HttpStatusCode.NotFound,
                    ex.Message);
            }

            // ===============================
            // Validation Exception
            // ===============================

            catch (ValidationException ex)
            {
                _logger.LogWarning(ex,
                    "Validation Exception");

                context.Response.ContentType =
                    "application/json";

                context.Response.StatusCode =
                    (int)HttpStatusCode.BadRequest;

                var response = new
                {
                    success = false,
                    statusCode = 400,
                    message =
                        "Validation failed",
                    errors = ex.Errors,
                    traceId =
                        context.TraceIdentifier
                };

                var json =
                    JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(json);
            }

            // ===============================
            // Resource Not Found Exception
            // ===============================

            catch (ResourceNotFoundException ex)
            {
                _logger.LogWarning(ex,
                    "Resource Not Found");

                await HandleExceptionAsync(
                    context,
                    HttpStatusCode.NotFound,
                    ex.Message);
            }

            // ===============================
            // Unauthorized Access
            // ===============================

            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex,
                    "Unauthorized Access");

                await HandleExceptionAsync(
                    context,
                    HttpStatusCode.Unauthorized,
                    ex.Message);
            }

            // ===============================
            // General Exception
            // ===============================

            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unhandled Exception");

                await HandleExceptionAsync(
                    context,
                    HttpStatusCode.InternalServerError,
                    "Something went wrong. Please try again later.");
            }
        }

        // ===============================
        // Common Exception Handler
        // ===============================

        private static async Task HandleExceptionAsync(
            HttpContext context,
            HttpStatusCode statusCode,
            string message)
        {
            context.Response.ContentType =
                "application/json";

            context.Response.StatusCode =
                (int)statusCode;

            var response = new
            {
                success = false,
                statusCode =
                    context.Response.StatusCode,
                message = message,
                traceId =
                    context.TraceIdentifier
            };

            var json =
                JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}