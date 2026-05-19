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
    {
        try
        {
            return Ok(await _service.GetAllAsync(page, pageSize));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
        }
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Employee,Customer")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var t = await _service.GetByIdAsync(id);
            return Ok(t);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
        }
    }

    [HttpGet("customer/{custId}")]
    [Authorize(Roles = "Admin,Employee,Customer")]
    public async Task<IActionResult> GetByCustomer(int custId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            return Ok(await _service.GetByCustomerAsync(custId, page, pageSize));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
        }
    }

    [HttpGet("pet/{petId}")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> GetByPet(int petId)
    {
        try
        {
            return Ok(await _service.GetByPetAsync(petId));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
        }
    }

    [HttpGet("total-revenue")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetTotalRevenue()
    {
        try
        {
            return Ok(new { TotalRevenue = await _service.GetTotalRevenueAsync() });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
        }
    }

    [HttpGet("summary")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetSummary()
    {
        try
        {
            return Ok(await _service.GetSalesSummaryAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Customer")]
    public async Task<IActionResult> Create([FromBody] CreateTransactionDto dto)
    {
        try
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.TransactionId }, created);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
        }
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateTransactionStatusDto dto)
    {
        try
        {
            var updated = await _service.UpdateStatusAsync(id, dto);
            return Ok(updated);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
        }
    }

    [HttpGet("search")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Search([FromQuery] string query, [FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var results = await _service.SearchAsync(query, status, page, pageSize);
            return Ok(results);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
        }
    }
}
