using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetManagementSystem.Api.Exceptions;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetManagementSystem.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred.");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = exception switch
            {
                // 404 Not Found
                NotFoundException => (int)HttpStatusCode.NotFound,
                ResourceNotFoundException => (int)HttpStatusCode.NotFound,
                EmployeeNotFoundException => (int)HttpStatusCode.NotFound,
                VaccinationNotFoundException => (int)HttpStatusCode.NotFound,
                DataNotFoundException => (int)HttpStatusCode.NotFound,
                SupplierNotFoundException => (int)HttpStatusCode.NotFound,

                // 400 Bad Request
                BadRequestException => (int)HttpStatusCode.BadRequest,
                ValidationException => (int)HttpStatusCode.BadRequest,
                EmployeeValidationException => (int)HttpStatusCode.BadRequest,
                VaccinationValidationException => (int)HttpStatusCode.BadRequest,
                ArgumentException => (int)HttpStatusCode.BadRequest,

                // 401 Unauthorized
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                InvalidCredentialsException => (int)HttpStatusCode.Unauthorized,

                // 500 Internal Server Error
                _ => (int)HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = statusCode;

            object response;

            if (exception is ValidationException validationException)
            {
                response = new
                {
                    success = false,
                    statusCode = statusCode,
                    message = "Validation failed",
                    errors = validationException.Errors,
                    traceId = context.TraceIdentifier
                };
            }
            else
            {
                response = new
                {
                    success = false,
                    statusCode = statusCode,
                    message = statusCode == (int)HttpStatusCode.InternalServerError
                        ? "Internal Server Error."
                        : exception.Message,
                    traceId = context.TraceIdentifier
                };
            }

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}