using Microsoft.AspNetCore.Mvc;
using PetManagementSystem.Web.Helpers;
using PetManagementSystem.Web.Models;
using PetManagementSystem.Web.Services;

namespace PetManagementSystem.Web.Controllers
{
    public class StaffController : Controller
    {
        private readonly IApiService _api;

        public StaffController(IApiService api)
        {
            _api = api;
        }

        // GET: /Staff/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var userId = AuthHelper.GetUserId(Request);

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var employee = await _api.GetAsync<EmployeeViewModel>($"employees/{userId}");

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        public async Task<IActionResult> Profile()
        {
            var userId = AuthHelper.GetUserId(Request);

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var employee = await _api.GetAsync<EmployeeViewModel>($"employees/{userId}");

            if (employee == null)
                return NotFound();

            return View(employee);
        }

    }
}