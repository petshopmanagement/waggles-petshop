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

    [Authorize(Roles = "Employee")]
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
    public async Task<IActionResult> Patch(int id, [FromBody] PatchCustomerDto dto)
    {
        return Ok(await _service.PatchAsync(id, dto));
    }

    [HttpPost("{id}/address")]
    public async Task<IActionResult> AddAddress(int id, [FromBody] AddressDto dto)
    {
        return Ok(await _service.AddAddressAsync(id, dto));
    }

    [Authorize(Roles = "Employee")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
