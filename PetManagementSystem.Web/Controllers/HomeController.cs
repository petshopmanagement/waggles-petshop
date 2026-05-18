using Microsoft.AspNetCore.Mvc;
using PetManagementSystem.Web.Models;
using PetManagementSystem.Web.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetManagementSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApiService _api;

        public HomeController(IApiService api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index(string search, int? categoryId, int page = 1)
        {
            ViewBag.CurrentPage = page;
            int pageSize = 10;

            var role = Helpers.AuthHelper.GetRole(Request);
            if (role == "Employee") return RedirectToAction("Dashboard", "Staff");
            if (role == "Supplier") return RedirectToAction("Dashboard", "Supplier");
            if (role == "Admin") return RedirectToAction("Dashboard", "Admin");

            IEnumerable<PetViewModel>? pets = null;

            // 1. Handle Search Query
            if (!string.IsNullOrEmpty(search))
            {
                pets = await _api.GetAsync<IEnumerable<PetViewModel>>($"pets/name/{search}?page={page}&pageSize={pageSize}");
            }
            // 2. Handle Category Filter
            else if (categoryId.HasValue && categoryId > 0)
            {
                pets = await _api.GetAsync<IEnumerable<PetViewModel>>($"pets/category/{categoryId}?page={page}&pageSize={pageSize}");
            }
            // 3. Default: Load All Pets
            else
            {
                pets = await _api.GetAsync<IEnumerable<PetViewModel>>($"pets?page={page}&pageSize={pageSize}");
            }

            return View(pets ?? new List<PetViewModel>());
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
