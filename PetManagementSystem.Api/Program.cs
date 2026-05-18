using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PetManagementSystem.Api.Data;
using PetManagementSystem.Api.Helpers;
using PetManagementSystem.Api.Mappings;
using PetManagementSystem.Api.Middleware;
using PetManagementSystem.Api.Repositories;
using PetManagementSystem.Api.Services;
using PetManagementSystem.Api.Validators;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;

namespace PetManagementSystem.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler =
                        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;

                    options.JsonSerializerOptions.DefaultIgnoreCondition =
                        System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                });

            builder.Services.AddDbContext<PetStoreDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();


            builder.Services.AddScoped<IPetRepository, PetRepository>();
            builder.Services.AddScoped<IFoodRepo, FoodRepo>();

            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IVaccinationRepository, VaccinationRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<ISupplierRepo, SupplierRepo>();
            builder.Services.AddScoped<IGroomingServiceRepo, GroomingServiceRepo>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();

            builder.Services.AddScoped<IPetService, PetService>();
            builder.Services.AddScoped<IFoodService, FoodService>();

            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IVaccinationService, VaccinationService>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<JwtHelper>();

            builder.Services.AddScoped<ISupplierService, SupplierService>();
            builder.Services.AddScoped<IGroomingServiceService, GroomingServiceService>();

            
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var secretKey =
                jwtSettings["Key"];

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme =
                    JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(secretKey)),

                        ValidateIssuer = true,
                        ValidIssuer =
                            jwtSettings["Issuer"] ??
                            "PetStoreApi",

                        ValidateAudience = true,
                        ValidAudience =
                            jwtSettings["Audience"] ??
                            "PetStoreClients",

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
            });

            
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Waggles Pet Shop API",
                        Version = "v1",
                        Description =
                            "Enterprise Pet Management System REST API"
                    });

                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description =
                            "JWT Authorization header using Bearer. Example: 'Bearer {token}'",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference =
                                    new OpenApiReference
                                    {
                                        Type =
                                            ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                            },
                            new List<string>()
                        }
                    });
            });

            
            var app = builder.Build();

            app.UseMiddleware<GlobalExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}