using Microsoft.AspNetCore.Mvc;
using PetManagementSystem.Web.Helpers;
using PetManagementSystem.Web.Models;
using PetManagementSystem.Web.Services;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetManagementSystem.Web.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly IApiService _api;

        public TransactionsController(IApiService api)
        {
            _api = api;
        }

        // Admin view of all transactions
        public async Task<IActionResult> Index(string search, string status)
        {
            string endpoint = "transactions";
            if (!string.IsNullOrEmpty(search) || (!string.IsNullOrEmpty(status) && status != "All"))
            {
                endpoint = $"transactions/search?query={search}&status={status}";
            }

            var response = await _api.GetAsync<JsonElement?>(endpoint);
            List<dynamic> transactions = new List<dynamic>();
            if (response != null && response.Value.TryGetProperty("data", out var data))
            {
                transactions = JsonSerializer.Deserialize<List<dynamic>>(data.GetRawText()) ?? new List<dynamic>();
            }
            else if (response != null && response.Value.ValueKind == JsonValueKind.Array)
            {
                // API might return array directly without "data" wrapper depending on endpoint
                transactions = JsonSerializer.Deserialize<List<dynamic>>(response.Value.GetRawText()) ?? new List<dynamic>();
            }

            var revenueResponse = await _api.GetAsync<JsonElement?>("transactions/total-revenue");
            ViewBag.TotalRevenue = revenueResponse?.GetProperty("totalRevenue").GetDecimal() ?? 0;

            return View(transactions);
        }

        // Customer Checkout Page
        [HttpGet]
        public async Task<IActionResult> Checkout(int petId)
        {
            if (!AuthHelper.IsLoggedIn(Request))
                return RedirectToAction("Login", "Auth", new { returnUrl = Url.Action("Checkout", "Transactions", new { petId }) });

            var pet = await _api.GetAsync<PetViewModel>($"pets/{petId}");
            if (pet == null) return NotFound();

            return View(pet);
        }

        // Process the Payment and Adoption
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(int petId, string paymentMethod, string paymentDetails)
        {
            int? customerId = AuthHelper.GetUserId(Request);
            if (customerId == null) return RedirectToAction("Login", "Auth");

            var pet = await _api.GetAsync<PetViewModel>($"pets/{petId}");
            if (pet == null) return RedirectToAction("Failure");

            // Create transaction payload using strongly typed DTO
            var payload = new CreateTransactionDto
            {
                CustomerId = customerId,
                PetId = petId,
                TransactionDate = DateOnly.FromDateTime(DateTime.Now),
                Amount = pet.Price ?? 0,
                TransactionStatus = "Success"
            };

            // Process the transaction via API
            var result = await _api.PostAsync<CreateTransactionDto, TransactionViewModel>("transactions", payload);

            if (result == null)
            {
                // In case of failure, we'll show the failure page for better feedback
                return RedirectToAction("Failure");
            }

            return RedirectToAction("Success", new { petName = pet.Name });
        }

        public IActionResult Success(string petName)
        {
            ViewBag.PetName = petName;
            return View();
        }

        public IActionResult Failure()
        {
            return View();
        }
    }
}
