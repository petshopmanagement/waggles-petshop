using Microsoft.AspNetCore.Mvc;
using PetManagementSystem.Web.Models;
using PetManagementSystem.Web.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetManagementSystem.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IApiService _api;

        public AdminController(IApiService api)
        {
            _api = api;
        }

        public async Task<IActionResult> Dashboard()
        {
            var employees = await _api.GetAsync<IEnumerable<EmployeeViewModel>>("employees");
            var customers = await _api.GetAsync<IEnumerable<CustomerViewModel>>("customers");
            var petCount = await _api.GetAsync<int>("pets/count");
            var food = await _api.GetAsync<IEnumerable<PetFoodViewModel>>("PetFood");

            var suppliersResp = await _api.GetAsync<JsonElement?>("Supplier");
            int supplierCount = 0;
            if (suppliersResp.HasValue &&
                suppliersResp.Value.TryGetProperty("data", out var supplierData) &&
                supplierData.ValueKind == JsonValueKind.Array)
            {
                supplierCount = supplierData.GetArrayLength();
            }

            ViewBag.EmployeeCount = employees?.Count() ?? 0;
            ViewBag.CustomerCount = customers?.Count() ?? 0;
            ViewBag.SupplierCount = supplierCount;
            ViewBag.PetCount = petCount;
            ViewBag.FoodCount = food?.Count() ?? 0;


            var transactions = await _api.GetAsync<IEnumerable<dynamic>>("transactions");
            return View(transactions?.Take(5).ToList() ?? new List<dynamic>());
        }

        public async Task<IActionResult> Employees()
        {
            var employees = await _api.GetAsync<IEnumerable<EmployeeViewModel>>("employees");
            return View(employees ?? Enumerable.Empty<EmployeeViewModel>());
        }

        public async Task<IActionResult> Customers()
        {
            var customers = await _api.GetAsync<IEnumerable<CustomerViewModel>>("customers");
            return View(customers ?? Enumerable.Empty<CustomerViewModel>());
        }

        public async Task<IActionResult> Suppliers()
        {
            var response = await _api.GetAsync<JsonElement?>("Supplier");
            var suppliers = new List<SupplierViewModel>();

            if (response.HasValue &&
                response.Value.TryGetProperty("data", out var data) &&
                data.ValueKind == JsonValueKind.Array)
            {
                suppliers = JsonSerializer.Deserialize<List<SupplierViewModel>>(
                    data.GetRawText(),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                ) ?? new List<SupplierViewModel>();
            }

            return View(suppliers);
        }

        public async Task<IActionResult> Services()
        {

            var groomingResp = await _api.GetAsync<JsonElement?>("GroomingServices");
            var groomingServices = new List<GroomingServiceViewModel>();
            if (groomingResp.HasValue && groomingResp.Value.TryGetProperty("data", out var gData) && gData.ValueKind == JsonValueKind.Array)
            {
                groomingServices = JsonSerializer.Deserialize<List<GroomingServiceViewModel>>(gData.GetRawText(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<GroomingServiceViewModel>();
            }


            var vaccinations = await _api.GetAsync<IEnumerable<VaccinationViewModel>>("Vaccinations") ?? new List<VaccinationViewModel>();

            var model = new AdminServicesViewModel
            {
                GroomingServices = groomingServices,
                Vaccinations = vaccinations
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGrooming(GroomingServiceViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Services");

            var payload = new
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Available = model.IsAvailable ? 1 : 0
            };

            await _api.PostAsync<object, dynamic>("GroomingServices", payload);
            TempData["SuccessMessage"] = "Grooming service created successfully!";
            return RedirectToAction("Services");
        }

        [HttpPost]
        public async Task<IActionResult> CreateVaccination(VaccinationViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Services");

            var payload = new
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Available = model.IsAvailable
            };

            await _api.PostAsync<object, dynamic>("Vaccinations", payload);
            TempData["SuccessMessage"] = "Vaccination added successfully!";
            return RedirectToAction("Services");
        }

        public async Task<IActionResult> PetFood()
        {
            var food = await _api.GetAsync<IEnumerable<PetFoodViewModel>>("PetFood");
            return View(food ?? Enumerable.Empty<PetFoodViewModel>());
        }

        [HttpPost]
        public async Task<IActionResult> CreatePetFood(PetFoodViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var food = await _api.GetAsync<IEnumerable<PetFoodViewModel>>("PetFood");
                return View("PetFood", food ?? Enumerable.Empty<PetFoodViewModel>());
            }

            var payload = new
            {
                Name = model.Name,
                Brand = model.Brand,
                Type = model.Type,
                Quantity = model.Quantity,
                Price = model.Price
            };

            await _api.PostAsync<object, dynamic>("PetFood", payload);
            TempData["SuccessMessage"] = "Pet food added successfully!";
            return RedirectToAction("PetFood");
        }
    }
}