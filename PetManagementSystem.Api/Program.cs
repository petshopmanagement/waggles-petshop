using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetManagementSystem.Api.Data;
using PetManagementSystem.Api.Repositories;
using PetManagementSystem.Api.Services;
using PetManagementSystem.Api.Validators;
using System.Text.Json.Serialization;
using PetManagementSystem.Api.Middleware;

namespace PetManagementSystem.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers().AddJsonOptions(option => option.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            builder.Services.AddDbContext<PetStoreDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IPetService, PetService>();
            builder.Services.AddScoped<IFoodRepo, FoodRepo>();

            builder.Services.AddScoped<IFoodService, FoodService>();

            builder.Services.AddScoped<IPetRepository, PetRepository>();

            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddValidatorsFromAssemblyContaining<PetCreateDTOValidator>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}