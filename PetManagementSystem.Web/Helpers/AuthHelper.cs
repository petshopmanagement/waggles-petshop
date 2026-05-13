using Microsoft.AspNetCore.Http;

namespace PetManagementSystem.Web.Helpers
{
    public static class AuthHelper
    {
        public static bool IsLoggedIn(HttpRequest request)
            => !string.IsNullOrEmpty(request.Cookies["auth_token"]);

        public static string? GetToken(HttpRequest request)
            => request.Cookies["auth_token"];

        public static string? GetRole(HttpRequest request)
            => request.Cookies["auth_role"];

        public static string? GetName(HttpRequest request)
            => request.Cookies["auth_name"];

        public static string? GetEmail(HttpRequest request)
            => request.Cookies["auth_email"];

        public static int? GetUserId(HttpRequest request)
        {
            var val = request.Cookies["auth_userid"];
            return int.TryParse(val, out var id) ? id : null;
        }

        public static bool IsAdmin(HttpRequest request)
            => GetRole(request)?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true;

        public static bool IsCustomer(HttpRequest request)
            => GetRole(request)?.Equals("Customer", StringComparison.OrdinalIgnoreCase) == true;
        public static bool IsEmployee(HttpRequest request)
            => GetRole(request)?.Equals("Employee", StringComparison.OrdinalIgnoreCase) == true;

    }
}
