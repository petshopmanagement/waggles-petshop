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
            var pets = await _api.GetAsync<IEnumerable<PetViewModel>>("pets");

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
            ViewBag.PetCount = pets?.Count() ?? 0;

            // Recent transactions for the dashboard table (last 5)
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
    }
}