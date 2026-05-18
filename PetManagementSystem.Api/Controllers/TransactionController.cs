using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Services;

namespace PetManagementSystem.Api.Controllers;

[ApiController]
[Route("api/transactions")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _service;

    public TransactionsController(ITransactionService service) => _service = service;

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        => Ok(await _service.GetAllAsync(page, pageSize));

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Employee,Customer")]
    public async Task<IActionResult> GetById(int id)
    {
        var t = await _service.GetByIdAsync(id);
        return Ok(t);
    }

    [HttpGet("customer/{custId}")]
    [Authorize(Roles = "Admin,Employee,Customer")]
    public async Task<IActionResult> GetByCustomer(int custId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        => Ok(await _service.GetByCustomerAsync(custId, page, pageSize));

    [HttpGet("pet/{petId}")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> GetByPet(int petId)
        => Ok(await _service.GetByPetAsync(petId));

    [HttpGet("total-revenue")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetTotalRevenue()
        => Ok(new { TotalRevenue = await _service.GetTotalRevenueAsync() });

    [HttpGet("summary")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetSummary()
        => Ok(await _service.GetSalesSummaryAsync());

    [HttpPost]
    [Authorize(Roles = "Admin,Customer")]
    public async Task<IActionResult> Create([FromBody] CreateTransactionDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.TransactionId }, created);
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateTransactionStatusDto dto)
    {
        var updated = await _service.UpdateStatusAsync(id, dto);
        return Ok(updated);
    }

    [HttpGet("search")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Search([FromQuery] string query, [FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var results = await _service.SearchAsync(query, status, page, pageSize);
        return Ok(results);
    }
}
