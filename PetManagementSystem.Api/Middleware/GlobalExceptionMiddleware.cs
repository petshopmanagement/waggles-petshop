using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using PetManagementSystem.Api.Exceptions;

namespace PetManagementSystem.Api.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var responseModel = new { success = false, message = error?.Message };

            switch (error)
            {
                case InvalidCredentialsException e:
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case InvalidRoleException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case EmailAlreadyExistsException e:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    break;
                case CustomerNotFoundException e:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case AppException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case ArgumentException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    responseModel = new { success = false, message = "An internal server error occurred." };
                    break;
            }

            var result = JsonSerializer.Serialize(responseModel);
            await response.WriteAsync(result);
        }
    }
}
