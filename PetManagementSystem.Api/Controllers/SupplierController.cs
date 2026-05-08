using Microsoft.AspNetCore.Http;
using PetManagementSystem.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using PetManagementSystem.Api.Helpers;
using PetManagementSystem.Api.Models;
using PetManagementSystem.Api.Services;

namespace PetManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {

        private readonly ISupplierService _supplierService;
        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            return Ok(suppliers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetSupplier(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null) return NotFound();
            return Ok(supplier);
        }


        [HttpPost]
        public async Task<ActionResult<Supplier>> PostSupplier(CreateSupplierDto supplier)
        {
            var createdSupplier = await _supplierService.CreateSupplierAsync(supplier);
            return CreatedAtAction(nameof(GetSupplier), new { id = createdSupplier.SupplierId }, createdSupplier);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupplier(int id, UpdateSupplierDto supplier)
        {
            try
            {
                await _supplierService.UpdateSupplierAsync(id, supplier);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            return NoContent();
        }
        [HttpGet("{id}/pets")]
        public async Task<ActionResult<IEnumerable<PetDto>>> GetAllPets(int id)
        {
            var petDtos = await _supplierService.GetPetsAsync(id);

            if (!petDtos.Any())
            {
                return NotFound();
            }

            return Ok(petDtos);
        }
    }
}