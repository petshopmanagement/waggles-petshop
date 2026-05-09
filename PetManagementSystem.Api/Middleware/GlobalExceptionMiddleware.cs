using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using PetManagementSystem.Api.Exceptions;

namespace PetManagementSystem.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(
                    context,
                    ex);
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            context.Response.ContentType =
                "application/json";

            var statusCode = exception switch
            {
                // ===============================
                // Not Found Exceptions
                // ===============================
                NotFoundException => (int)HttpStatusCode.NotFound,
                ResourceNotFoundException => (int)HttpStatusCode.NotFound,
                EmployeeNotFoundException => (int)HttpStatusCode.NotFound,
                VaccinationNotFoundException => (int)HttpStatusCode.NotFound,

                // ===============================
                // Bad Request Exceptions
                // ===============================
                BadRequestException => (int)HttpStatusCode.BadRequest,
                ValidationException => (int)HttpStatusCode.BadRequest,
                EmployeeValidationException => (int)HttpStatusCode.BadRequest,
                VaccinationValidationException => (int)HttpStatusCode.BadRequest,
                ArgumentException => (int)HttpStatusCode.BadRequest,

                // ===============================
                // Unauthorized Access
                // ===============================
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,

                // ===============================
                // Internal Server Error
                // ===============================
                _ => (int)HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode =
                statusCode;

            object response;

            // ===============================
            // Special Handling for ValidationException
            // ===============================
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
                    message =
                        statusCode ==
                        (int)HttpStatusCode.InternalServerError
                            ? "Internal Server Error."
                            : exception.Message,
                    traceId =
                        context.TraceIdentifier
                };
            }

            var json =
                JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(
                json);
        }
    }
}