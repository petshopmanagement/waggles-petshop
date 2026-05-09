using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Services;

namespace PetManagementSystem.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _service;
    public CustomersController(ICustomerService service) => _service = service;

    [Authorize(Roles = "Employee, Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await _service.GetByIdAsync(id);
        return customer == null ? NotFound() : Ok(customer);
    }

    [HttpGet("{id}/profile")]
    public async Task<IActionResult> GetProfile(int id)
    {
        var profile = await _service.GetProfileAsync(id);
        return profile == null ? NotFound() : Ok(profile);
    }

    [HttpGet("{id}/transactions")]
    public async Task<IActionResult> GetTransactions(int id)
        => Ok(await _service.GetTransactionsAsync(id));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch(int id, [FromBody] PatchCustomerDto dto)
    {
        var updated = await _service.PatchAsync(id, dto);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpPost("{id}/address")]
    public async Task<IActionResult> AddAddress(int id, [FromBody] AddressDto dto)
    {
        var updatedProfile = await _service.AddAddressAsync(id, dto);
        return updatedProfile == null ? NotFound() : Ok(updatedProfile);
    }

    [Authorize(Roles = "Employee,Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
