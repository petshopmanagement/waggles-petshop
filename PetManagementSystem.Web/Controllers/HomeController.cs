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

        public async Task<IActionResult> Index(string search, int? categoryId)
        {
            IEnumerable<PetViewModel>? pets = null;

            // 1. Handle Search Query
            if (!string.IsNullOrEmpty(search))
            {
                pets = await _api.GetAsync<IEnumerable<PetViewModel>>($"pets/name/{search}");
            }
            // 2. Handle Category Filter
            else if (categoryId.HasValue && categoryId > 0)
            {
                pets = await _api.GetAsync<IEnumerable<PetViewModel>>($"pets/category/{categoryId}");
            }
            // 3. Default: Load All Pets
            else
            {
                pets = await _api.GetAsync<IEnumerable<PetViewModel>>("pets");
            }

            return View(pets ?? new List<PetViewModel>());
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
