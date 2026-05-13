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
            // 1. Fetch total pets
            var pets = await _api.GetAsync<IEnumerable<PetViewModel>>("pets");
            ViewBag.PetCount = pets?.Count() ?? 0;

            // 2. Fetch revenue stats (using existing endpoints)
            var revenueResponse = await _api.GetAsync<JsonElement?>("transactions/total-revenue");
            ViewBag.TotalRevenue = revenueResponse?.GetProperty("totalRevenue").GetDecimal() ?? 0;

            // 3. Fetch sales summary (success/fail counts)
            var summary = await _api.GetAsync<JsonElement?>("transactions/summary");
            ViewBag.Summary = summary;

            // 4. Fetch recent transactions for the table
            var transactions = await _api.GetAsync<IEnumerable<dynamic>>("transactions");
            
            return View(transactions?.Take(5).ToList() ?? new List<dynamic>());
        }
    }
}
