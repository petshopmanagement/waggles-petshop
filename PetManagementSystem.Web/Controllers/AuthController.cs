using Microsoft.AspNetCore.Mvc;
using PetManagementSystem.Web.Models;
using PetManagementSystem.Web.Services;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetManagementSystem.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IApiService _api;

        public AuthController(IApiService api)
        {
            _api = api;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var payload = new
            {
                Email = model.Email,
                Password = model.Password,
                Role = model.Role
            };

            var response = await _api.PostAsync<object, JsonElement?>("auth/login", payload);

            if (response == null || response.Value.ValueKind == JsonValueKind.Null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your credentials.");
                return View(model);
            }

            try
            {
                var root = response.Value;
                
                // 1. Check if success is true (try both casing)
                bool isSuccess = false;
                if (root.TryGetProperty("success", out var s1)) isSuccess = s1.GetBoolean();
                else if (root.TryGetProperty("Success", out var s2)) isSuccess = s2.GetBoolean();

                if (!isSuccess)
                {
                    string error = "Invalid login attempt.";
                    if (root.TryGetProperty("message", out var m1)) error = m1.GetString() ?? error;
                    else if (root.TryGetProperty("Message", out var m2)) error = m2.GetString() ?? error;
                    
                    ModelState.AddModelError(string.Empty, error);
                    return View(model);
                }

                // 2. Extract Data object
                JsonElement data;
                if (root.TryGetProperty("data", out var d1)) data = d1;
                else if (root.TryGetProperty("Data", out var d2)) data = d2;
                else throw new Exception("Data property missing");

                // 3. Extract properties from Data (try both casing)
                string? token = GetProp(data, "token", "Token");
                string? role = GetProp(data, "role", "Role");
                string? name = GetProp(data, "name", "Name");
                string? email = GetProp(data, "email", "Email");
                string? userId = GetProp(data, "userId", "UserId");

                // 4. Set cookies
                var cookieOptions = new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Lax, Expires = DateTime.Now.AddHours(1) };
                var publicCookieOptions = new CookieOptions { HttpOnly = false, SameSite = SameSiteMode.Lax, Expires = DateTime.Now.AddHours(1) };

                Response.Cookies.Append("auth_token", token ?? "", cookieOptions);
                Response.Cookies.Append("auth_role", role ?? "", publicCookieOptions);
                Response.Cookies.Append("auth_name", name ?? "", publicCookieOptions);
                Response.Cookies.Append("auth_email", email ?? "", publicCookieOptions);
                Response.Cookies.Append("auth_userid", userId ?? "", cookieOptions);

                TempData["SuccessMessage"] = $"Welcome back, {name}!";

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred during login. Please try again.");
                return View(model);
            }
        }

        private string? GetProp(JsonElement el, string lower, string upper)
        {
            if (el.TryGetProperty(lower, out var v1)) return v1.GetString();
            if (el.TryGetProperty(upper, out var v2)) return v2.GetString();
            return null;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Structure payload based on API expectations
            var payload = new
            {
                Email = model.Email,
                Password = model.Password,
                Role = model.Role,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Name = model.Name,
                ContactPerson = model.ContactPerson,
                Position = model.Position,
                PhoneNumber = model.PhoneNumber,
                Address = new
                {
                    Street = model.Street,
                    City = model.City,
                    State = model.State,
                    ZipCode = model.ZipCode
                }
            };

            var response = await _api.PostAsync<object, JsonElement?>("auth/register", payload);

            if (response == null)
            {
                ModelState.AddModelError(string.Empty, "The server is currently unavailable. Please try again later.");
                return View(model);
            }

            // 1. Handle FluentValidation / Custom Middleware Errors (List of Strings)
            if (response.Value.ValueKind == JsonValueKind.Object && response.Value.TryGetProperty("errors", out var apiErrors))
            {
                if (apiErrors.ValueKind == JsonValueKind.Array)
                {
                    // Case: ["Error 1", "Error 2"]
                    foreach (var error in apiErrors.EnumerateArray())
                    {
                        ModelState.AddModelError(string.Empty, error.GetString() ?? "Validation error.");
                    }
                }
                else if (apiErrors.ValueKind == JsonValueKind.Object)
                {
                    // Case: { "Field": ["Error 1"] }
                    foreach (var errorField in apiErrors.EnumerateObject())
                    {
                        foreach (var errorMessage in errorField.Value.EnumerateArray())
                        {
                            ModelState.AddModelError(string.Empty, $"{errorField.Name}: {errorMessage.GetString()}");
                        }
                    }
                }
                return View(model);
            }

            // 2. Handle Custom API Logic Errors (success: false)
            if (response.Value.TryGetProperty("success", out var success) && !success.GetBoolean())
            {
                var error = response.Value.TryGetProperty("message", out var msg) ? msg.GetString() : "Registration failed.";
                ModelState.AddModelError(string.Empty, error ?? "Registration failed.");
                return View(model);
            }

            TempData["SuccessMessage"] = "Registration successful! Please login.";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("auth_token");
            Response.Cookies.Delete("auth_role");
            Response.Cookies.Delete("auth_name");
            Response.Cookies.Delete("auth_email");
            Response.Cookies.Delete("auth_userid");
            return RedirectToAction("Index", "Home");
        }
    }
}
