using Microsoft.AspNetCore.Mvc;
using PetManagementSystem.Web.Helpers;
using PetManagementSystem.Web.Models;
using PetManagementSystem.Web.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetManagementSystem.Web.Controllers
{
    public class PetsController : Controller
    {
        private readonly IApiService _api;

        public PetsController(IApiService api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Details(int id)
        {
            var pet = await _api.GetAsync<PetViewModel>($"pets/{id}");
            if (pet == null) return NotFound();

            // Fetch related data
            var vaccinations = await _api.GetAsync<IEnumerable<System.Text.Json.JsonElement>>($"pets/vaccinations/{id}");
            var groomings = await _api.GetAsync<IEnumerable<System.Text.Json.JsonElement>>($"pets/groomings/{id}");
            var foods = await _api.GetAsync<IEnumerable<System.Text.Json.JsonElement>>($"PetFood/pet/{id}");

            ViewBag.Vaccinations = vaccinations ?? new List<System.Text.Json.JsonElement>();
            ViewBag.Groomings = groomings ?? new List<System.Text.Json.JsonElement>();
            ViewBag.Foods = foods ?? new List<System.Text.Json.JsonElement>();
            
            return View(pet);
        }
    }
}
