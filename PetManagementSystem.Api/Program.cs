using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using PetManagementSystem.Api.Data;
using PetManagementSystem.Api.Repositories;
using PetManagementSystem.Api.Services;
using System.Text.Json.Serialization;

using PetManagementSystem.Api.Middleware;
using Serilog;

namespace PetManagementSystem.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.AddControllers()
     .AddNewtonsoftJson()
     .AddJsonOptions(opts =>
     {

         opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
     });

            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();
            
            builder.Services.AddDbContext<PetStoreDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<ISupplierService, SupplierService>();
            builder.Services.AddScoped<ISupplierRepo, SupplierRepo>();
            builder.Services.AddScoped<IGroomingServiceService, GroomingServiceService>();
            builder.Services.AddScoped<IGroomingServiceRepo, GroomingServiceRepo>();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}