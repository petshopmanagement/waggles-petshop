using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using PetManagementSystem.Api.Exceptions;

namespace PetManagementSystem.Api.Middleware
{
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

        public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
            catch (Exception ex)
        {
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
                EmployeeNotFoundException => (int)HttpStatusCode.NotFound,
                VaccinationNotFoundException => (int)HttpStatusCode.NotFound,

                EmployeeValidationException => (int)HttpStatusCode.BadRequest,
                VaccinationValidationException => (int)HttpStatusCode.BadRequest,
                ArgumentException => (int)HttpStatusCode.BadRequest,

                _ => (int)HttpStatusCode.InternalServerError
            };

            context.Response.StatusCode = statusCode;

            var response = new
            {
                success = false,
                message = statusCode == (int)HttpStatusCode.InternalServerError
                    ? "An internal server error occurred."
                    : exception.Message
            };

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}
