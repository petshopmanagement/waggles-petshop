
using Microsoft.AspNetCore.Mvc;
using PetManagementSystem.Web.Helpers;
using PetManagementSystem.Web.Models;
using PetManagementSystem.Web.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetManagementSystem.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IApiService _api;

        public CustomerController(IApiService api)
        {
            _api = api;
        }

        public async Task<IActionResult> Dashboard()
        {
            int? userId = AuthHelper.GetUserId(Request);
            if (userId == null) return RedirectToAction("Login", "Auth");

            var profile = await _api.GetAsync<CustomerViewModel>($"customers/{userId}/profile");
            var transactions = await _api.GetAsync<IEnumerable<TransactionViewModel>>($"transactions/customer/{userId}?page=1&pageSize=10");

            var viewModel = new CustomerDashboardViewModel
            {
                Profile = profile,
                Transactions = transactions ?? new List<TransactionViewModel>()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Services()
        {
            // Fetch available grooming and vaccination options for the customer to browse
            var grooming = await _api.GetAsync<IEnumerable<dynamic>>("groomingservices");
            var vaccinations = await _api.GetAsync<IEnumerable<dynamic>>("vaccinations");

            ViewBag.Grooming = grooming ?? new List<dynamic>();
            ViewBag.Vaccinations = vaccinations ?? new List<dynamic>();

            return View();
        }

        public async Task<IActionResult> Transactions(int page = 1)
        {
            int? userId = AuthHelper.GetUserId(Request);
            if (userId == null) return RedirectToAction("Login", "Auth");

            ViewBag.CurrentPage = page;
            int pageSize = 10;
            var transactions = await _api.GetAsync<IEnumerable<TransactionViewModel>>($"transactions/customer/{userId}?page={page}&pageSize={pageSize}");
            return View(transactions ?? new List<TransactionViewModel>());
        }
    }
}
