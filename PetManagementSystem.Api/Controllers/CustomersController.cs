using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Services;
using System.Security.Claims;


namespace PetManagementSystem.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _service;
    public CustomersController(ICustomerService service) => _service = service;

    [HttpGet("profile/me")]
    public async Task<IActionResult> GetCurrentProfile()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            return Unauthorized(new { message = "Invalid or missing token." });

        var customer = await _service.GetProfileAsync(userId);
        if (customer == null)
            return NotFound(new { message = "Customer profile not found." });

        return Ok(customer);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    [HttpGet("{id}/profile")]
    public async Task<IActionResult> GetProfile(int id)
    {
        return Ok(await _service.GetProfileAsync(id));
    }
    

    [HttpGet("{id}/transactions")]
    public async Task<IActionResult> GetTransactions(int id)
        => Ok(await _service.GetTransactionsAsync(id));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerDto dto)
    {
        return Ok(await _service.UpdateAsync(id, dto));
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(int id, [FromBody] UpdateCustomerDto dto)
    {
        return Ok(await _service.PatchAsync(id, dto));
    }

    [HttpPost("{id}/Addaddress")]
    public async Task<IActionResult> AddAddress(int id, [FromBody] AddressDto dto)
    {
        return Ok(await _service.AddAddressAsync(id, dto));
    }



}