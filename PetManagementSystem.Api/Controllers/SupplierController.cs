using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Helpers;
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
        public async Task<ActionResult<ApiResponse<IEnumerable<SupplierDTO>>>> GetSuppliers()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            return Ok(ApiResponse<IEnumerable<SupplierDTO>>.SuccessResponse(suppliers));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<SupplierDTO>>> GetSupplier(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            return Ok(ApiResponse<SupplierDTO>.SuccessResponse(supplier));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<SupplierDTO>>> PostSupplier(SupplierDTO supplier)
        {
            var createdSupplier = await _supplierService.CreateSupplierAsync(supplier);
            return CreatedAtAction(nameof(GetSupplier), new { id = createdSupplier.SupplierId }, ApiResponse<SupplierDTO>.SuccessResponse(createdSupplier));
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> PatchSupplier(int id, [FromBody] JsonPatchDocument<SupplierDTO> patchDoc)
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
    }
}
