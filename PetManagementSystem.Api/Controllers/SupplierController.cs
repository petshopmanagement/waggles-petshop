using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Helpers;
using PetManagementSystem.Api.Services;

namespace PetManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<SupplierDto>>>> GetSuppliers()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            return Ok(ApiResponse<IEnumerable<SupplierDto>>.SuccessResponse(suppliers));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<SupplierDto>>> GetSupplier(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            return Ok(ApiResponse<SupplierDto>.SuccessResponse(supplier));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<SupplierDto>>> PostSupplier(SupplierDto supplier)
        {
            var createdSupplier = await _supplierService.CreateSupplierAsync(supplier);
            return CreatedAtAction(nameof(GetSupplier), new { id = createdSupplier.SupplierId }, ApiResponse<SupplierDto>.SuccessResponse(createdSupplier));
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> PatchSupplier(int id, [FromBody] JsonPatchDocument<SupplierDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest(ApiResponse<string>.FailureResponse("Invalid patch document."));

            await _supplierService.PatchSupplierAsync(id, patchDoc);
            return Ok(ApiResponse<string>.SuccessResponse("Supplier updated successfully."));
        }

        [HttpGet("{id}/pets")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PetDto>>>> GetAllPets(int id)
        {
            var petDtos = await _supplierService.GetPetsAsync(id);
            return Ok(ApiResponse<IEnumerable<PetDto>>.SuccessResponse(petDtos));
        }

        [HttpGet("{id}/address")]
        public async Task<ActionResult<ApiResponse<AddressDto>>> GetAddress(int id)
        {
            var addressDto = await _supplierService.GetAddressAsync(id);
            return Ok(ApiResponse<AddressDto>.SuccessResponse(addressDto));
        }
    }
}
