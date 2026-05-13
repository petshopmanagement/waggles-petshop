using Microsoft.AspNetCore.Mvc;
using PetManagementSystem.Web.Models;
using PetManagementSystem.Web.Services;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetManagementSystem.Web.Controllers
{
    public class StaffController : Controller
    {
        private readonly IApiService _api;

        public StaffController(IApiService api)
        {
            _api = api;
        }

        public async Task<IActionResult> Dashboard()
        {
            // Employees see a similar overview to Admin
            return RedirectToAction("Dashboard", "Admin");
        }

        public async Task<IActionResult> Inventory(string search, int? categoryId)
        {
            IEnumerable<PetViewModel>? pets = null;

            if (!string.IsNullOrEmpty(search))
            {
                pets = await _api.GetAsync<IEnumerable<PetViewModel>>($"pets/name/{search}");
            }
            else if (categoryId.HasValue && categoryId > 0)
            {
                pets = await _api.GetAsync<IEnumerable<PetViewModel>>($"pets/category/{categoryId}");
            }
            else
            {
                pets = await _api.GetAsync<IEnumerable<PetViewModel>>("pets");
            }

            return View(pets ?? new List<PetViewModel>());
        }

        public async Task<IActionResult> Services()
        {
            // Fetch all grooming services and vaccinations
            var groomingResponse = await _api.GetAsync<JsonElement?>("GroomingServices");
            var vaccinationResponse = await _api.GetAsync<JsonElement?>("Vaccinations");

            ViewBag.Grooming = groomingResponse;
            ViewBag.Vaccinations = vaccinationResponse;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditPet(int id)
        {
            var pet = await _api.GetAsync<PetViewModel>($"pets/{id}");
            if (pet == null) return NotFound();
            return View(pet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPet(PetViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _api.PutAsync<PetViewModel, dynamic>($"pets/{model.PetId}", model);
            if (result == null)
            {
                TempData["ErrorMessage"] = "Failed to update pet details.";
                return View(model);
            }

            TempData["SuccessMessage"] = "Pet updated successfully!";
            return RedirectToAction("Inventory");
        }
    }
}
