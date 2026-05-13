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

            string endpoint = $"Supplier/{supplierId}/pets";
            
            // If search or category is provided, use the search endpoint
            if (!string.IsNullOrEmpty(search) || (categoryId.HasValue && categoryId > 0))
            {
                endpoint = $"Supplier/{supplierId}/pets/search?query={search}&categoryId={categoryId ?? 0}";
            }

            var response = await _api.GetAsync<JsonElement?>(endpoint);
            
            List<PetViewModel> pets = new List<PetViewModel>();
            
            if (response != null && response.Value.TryGetProperty("data", out var data))
            {
                pets = JsonSerializer.Deserialize<List<PetViewModel>>(data.GetRawText()) ?? new List<PetViewModel>();
            }

            return View(pets);
        }

        [HttpGet]
        public IActionResult AddPet()
        {
            return View(new PetViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPet(PetViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            int? supplierId = AuthHelper.GetUserId(Request);
            
            // For a beginner project, we'll assume the API allows creating a pet 
            // and linking it to the supplier. 
            // In a real system, we'd call a specific "SupplyPet" endpoint.
            
            var payload = new
            {
                name = model.Name,
                breed = model.Breed,
                age = model.Age,
                price = model.Price,
                description = model.Description,
                imageUrl = model.ImageUrl,
                supplierId = supplierId // API might use this to create the relationship
            };

            var result = await _api.PostAsync<object, dynamic>("Pets", payload);

            if (result == null)
            {
                TempData["ErrorMessage"] = "Failed to add pet to inventory.";
                return View(model);
            }

            TempData["SuccessMessage"] = "Pet successfully added to Waggles inventory!";
            return RedirectToAction("Dashboard");
        }
    }
}
