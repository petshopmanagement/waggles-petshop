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
            
            var pets = await _api.GetAsync<IEnumerable<PetViewModel>>("pets");
            ViewBag.ActivePets = pets?.Take(5).ToList() ?? new List<PetViewModel>();

            return View(transactions?.Take(5).ToList() ?? new List<dynamic>());
        }

        public async Task<IActionResult> Pets(int page = 1)
        {
            int pageSize = 10;
            var pets = await _api.GetAsync<IEnumerable<PetViewModel>>($"pets?page={page}&pageSize={pageSize}");
            var totalPets = await _api.GetAsync<int>("pets/count");

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalPets / (double)pageSize);
            ViewBag.TotalPets = totalPets;

            return View(pets ?? Enumerable.Empty<PetViewModel>());
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
            if (!ModelState.IsValid) return await Services();

            var payload = new
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Available = model.IsAvailable ? 1 : 0
            };

            var response = await _api.PostAsync<object, JsonElement?>("GroomingServices", payload);
            if (HandleApiErrors(response))
            {
                return await Services();
            }

            TempData["SuccessMessage"] = "Grooming service created successfully!";
            return RedirectToAction("Services");
        }

        [HttpPost]
        public async Task<IActionResult> CreateVaccination(VaccinationViewModel model)
        {
            if (!ModelState.IsValid) return await Services();

            var payload = new
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Available = model.IsAvailable
            };

            var response = await _api.PostAsync<object, JsonElement?>("Vaccinations", payload);
            if (HandleApiErrors(response))
            {
                return await Services();
            }

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

            var response = await _api.PostAsync<object, JsonElement?>("PetFood", payload);
            if (HandleApiErrors(response))
            {
                var food = await _api.GetAsync<IEnumerable<PetFoodViewModel>>("PetFood");
                return View("PetFood", food ?? Enumerable.Empty<PetFoodViewModel>());
            }

            TempData["SuccessMessage"] = "Pet food added successfully!";
            return RedirectToAction("PetFood");
        }
        private bool HandleApiErrors(JsonElement? response)
        {
            if (response == null)
            {
                ModelState.AddModelError(string.Empty, "The server is currently unavailable.");
                return true;
            }

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