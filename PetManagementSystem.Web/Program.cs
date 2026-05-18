using PetManagementSystem.Web.Services;

namespace PetManagementSystem.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // 1. Add HttpContextAccessor (needed to read cookies from services)
            builder.Services.AddHttpContextAccessor();

            // 2. Add Token Handler for HttpClient
            builder.Services.AddTransient<BearerTokenHandler>();

            // 3. Configure HttpClient for ApiService
            var apiBaseUrl = builder.Configuration.GetValue<string>("ApiBaseUrl") ?? "https://localhost:7294/api/";
            builder.Services.AddHttpClient<IApiService, ApiService>(client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
            }).AddHttpMessageHandler<BearerTokenHandler>();

            var app = builder.Build();

            



            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();

            
        }
    }
}
