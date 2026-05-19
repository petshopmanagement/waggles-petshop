using Microsoft.AspNetCore.Mvc;
using PetManagementSystem.Web.Helpers;
using PetManagementSystem.Web.Models;
using PetManagementSystem.Web.Services;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetManagementSystem.Web.Controllers
{
    public class SupplierController : Controller
    {
        private readonly IApiService _api;

        public SupplierController(IApiService api)
        {
            _api = api;
        }

        public async Task<IActionResult> Dashboard(string search, int? categoryId)
        {
            int? supplierId = AuthHelper.GetUserId(Request);
            if (supplierId == null) return RedirectToAction("Login", "Auth");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            // Fetch Categories for the dropdown
            var catResponse = await _api.GetAsync<JsonElement?>("Categories");
            var categories = new List<CategoryViewModel>();
            if (catResponse != null)
            {
                JsonElement catData = default;
                if (catResponse.Value.TryGetProperty("data", out catData) || catResponse.Value.TryGetProperty("Data", out catData))
                {
                    categories = JsonSerializer.Deserialize<List<CategoryViewModel>>(catData.GetRawText(), options) ?? new List<CategoryViewModel>();
                }
            }
            ViewBag.Categories = categories;

            string endpoint = $"Supplier/{supplierId}/pets";
            
            // If search or category is provided, use the search endpoint
            if (!string.IsNullOrEmpty(search) || (categoryId.HasValue && categoryId > 0))
            {
                endpoint = $"Supplier/{supplierId}/pets/search?query={search}&categoryId={categoryId ?? 0}";
            }

            var response = await _api.GetAsync<JsonElement?>(endpoint);
            List<PetViewModel> pets = new List<PetViewModel>();
            
            if (response != null)
            {
                JsonElement petData = default;
                if (response.Value.TryGetProperty("data", out petData) || response.Value.TryGetProperty("Data", out petData))
                {
                    pets = JsonSerializer.Deserialize<List<PetViewModel>>(petData.GetRawText(), options) ?? new List<PetViewModel>();
                }
            }

            return View(pets);
        }

        public async Task<IActionResult> Profile()
        {
            int? supplierId = AuthHelper.GetUserId(Request);
            if (supplierId == null) return RedirectToAction("Login", "Auth");

            var response = await _api.GetAsync<JsonElement?>($"Supplier/{supplierId}");
            
            SupplierViewModel supplier = new SupplierViewModel();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            if (response != null && response.Value.TryGetProperty("data", out var data))
            {
                supplier = JsonSerializer.Deserialize<SupplierViewModel>(data.GetRawText(), options) ?? new SupplierViewModel();
            }

            return View(supplier);
        }

        [HttpGet]
        public async Task<IActionResult> AddPet()
        {
            var response = await _api.GetAsync<JsonElement?>("Categories");
            var categories = new List<CategoryViewModel>();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            if (response != null)
            {
                JsonElement dataElement = default;
                if (response.Value.TryGetProperty("data", out dataElement) || response.Value.TryGetProperty("Data", out dataElement))
                {
                    categories = JsonSerializer.Deserialize<List<CategoryViewModel>>(dataElement.GetRawText(), options) ?? new List<CategoryViewModel>();
                }
            }

            ViewBag.Categories = categories;
            return View(new PetViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPet(PetViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Re-fetch categories if model state is invalid
                var catResponse = await _api.GetAsync<JsonElement?>("Categories");
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                if (catResponse != null)
                {
                    JsonElement dataElement = default;
                    if (catResponse.Value.TryGetProperty("data", out dataElement) || catResponse.Value.TryGetProperty("Data", out dataElement))
                    {
                        ViewBag.Categories = JsonSerializer.Deserialize<List<CategoryViewModel>>(dataElement.GetRawText(), options);
                    }
                }
                return View(model);
            }

            int? supplierId = AuthHelper.GetUserId(Request);
            if (supplierId == null) return RedirectToAction("Login", "Auth");
            
            var payload = new
            {
                name = model.Name,
                breed = model.Breed,
                age = model.Age,
                price = model.Price,
                description = model.Description,
                imageUrl = model.ImageUrl,
                categoryId = model.CategoryId
            };

            var result = await _api.PostAsync<object, JsonElement?>($"Supplier/{supplierId}/pets", payload);

            bool isSuccess = false;
            if (result != null)
            {
                if (result.Value.TryGetProperty("success", out var s1))
                {
                    isSuccess = s1.ValueKind == JsonValueKind.True;
                }
                else if (result.Value.TryGetProperty("Success", out var s2))
                {
                    isSuccess = s2.ValueKind == JsonValueKind.True;
                }
                else if (result.Value.TryGetProperty("errors", out var err) || (result.Value.TryGetProperty("status", out var st) && st.GetInt32() >= 400))
                {
                    isSuccess = false;
                }
                else
                {
                    isSuccess = true;
                }
            }

            if (!isSuccess)
            {
                if (!HandleApiErrors(result))
                {
                    ModelState.AddModelError(string.Empty, "Failed to add pet to inventory.");
                }
                var catResponse = await _api.GetAsync<JsonElement?>("Categories");
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                if (catResponse != null)
                {
                    JsonElement dataElement = default;
                    if (catResponse.Value.TryGetProperty("data", out dataElement) || catResponse.Value.TryGetProperty("Data", out dataElement))
                    {
                        ViewBag.Categories = JsonSerializer.Deserialize<List<CategoryViewModel>>(dataElement.GetRawText(), options);
                    }
                }
                return View(model);
            }

            TempData["SuccessMessage"] = "Pet successfully added to Waggles inventory!";
            return RedirectToAction("Dashboard");
        }
        private bool HandleApiErrors(JsonElement? response)
        {
            if (response == null) return false;

            if (response.Value.ValueKind == JsonValueKind.Object && response.Value.TryGetProperty("errors", out var apiErrors))
            {
                if (apiErrors.ValueKind == JsonValueKind.Array)
                {
                    foreach (var error in apiErrors.EnumerateArray())
                    {
                        ModelState.AddModelError(string.Empty, error.GetString() ?? "Validation error.");
                    }
                }
                else if (apiErrors.ValueKind == JsonValueKind.Object)
                {
                    foreach (var errorField in apiErrors.EnumerateObject())
                    {
                        foreach (var errorMessage in errorField.Value.EnumerateArray())
                        {
                            ModelState.AddModelError(string.Empty, $"{errorField.Name}: {errorMessage.GetString()}");
                        }
                    }
                }
                return true;
            }

            if (response.Value.ValueKind == JsonValueKind.Object && response.Value.TryGetProperty("status", out var status) && status.GetInt32() >= 400)
            {
                if (response.Value.TryGetProperty("title", out var title))
                {
                    ModelState.AddModelError(string.Empty, title.GetString() ?? "An error occurred.");
                    return true;
                }
            }

            return false;
        }
    }
}
