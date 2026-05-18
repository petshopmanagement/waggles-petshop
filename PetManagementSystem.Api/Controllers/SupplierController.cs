using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Helpers;
using PetManagementSystem.Api.Services;
using System.Security.Claims;

namespace PetManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }
        
        [HttpGet("profile/me")]
        public async Task<ActionResult<ApiResponse<SupplierDTO>>> GetMyProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(ApiResponse<SupplierDTO>.FailureResponse("Invalid or missing token."));

            var supplier = await _supplierService.GetSupplierByIdAsync(userId);
            return Ok(ApiResponse<SupplierDTO>.SuccessResponse(supplier));
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SupplierDTO>>>> GetSuppliers()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            return Ok(ApiResponse<IEnumerable<SupplierDTO>>.SuccessResponse(suppliers));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Employee,Supplier")]
        public async Task<ActionResult<ApiResponse<SupplierDTO>>> GetSupplier(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            return Ok(ApiResponse<SupplierDTO>.SuccessResponse(supplier));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<SupplierDTO>>> PostSupplier(SupplierDTO supplier)
        {
            var createdSupplier = await _supplierService.CreateSupplierAsync(supplier);
            return CreatedAtAction(nameof(GetSupplier), new { id = createdSupplier.SupplierId }, ApiResponse<SupplierDTO>.SuccessResponse(createdSupplier));
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin,Supplier")]
        public async Task<ActionResult<ApiResponse<string>>> PatchSupplier(int id, [FromBody] JsonPatchDocument<SupplierDTO> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest(ApiResponse<string>.FailureResponse("Invalid patch document."));

            await _supplierService.PatchSupplierAsync(id, patchDoc);
            return Ok(ApiResponse<string>.SuccessResponse("Supplier updated successfully."));
        }

        [HttpGet("{id}/pets")]
        [Authorize(Roles = "Admin,Employee,Supplier")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PetDto>>>> GetAllPets(int id)
        {
            var petDtos = await _supplierService.GetPetsAsync(id);
            return Ok(ApiResponse<IEnumerable<PetDto>>.SuccessResponse(petDtos));
        }

        [HttpGet("{id}/pets/search")]
        [Authorize(Roles = "Admin,Employee,Supplier")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PetDto>>>> SearchPets(int id, [FromQuery] string query, [FromQuery] int? categoryId)
        {
            var petDtos = await _supplierService.SearchPetsAsync(id, query, categoryId);
            return Ok(ApiResponse<IEnumerable<PetDto>>.SuccessResponse(petDtos));
        }

        [HttpPost("{id}/pets")]
        [Authorize(Roles = "Admin,Supplier")]
        public async Task<ActionResult<ApiResponse<PetDto>>> AddPet(int id, [FromBody] PetCreate dto)
        {
            var createdPet = await _supplierService.AddPetAsync(id, dto);
            return Ok(ApiResponse<PetDto>.SuccessResponse(createdPet));
        }
    }
}
