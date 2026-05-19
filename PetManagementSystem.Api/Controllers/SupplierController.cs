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
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                    return Unauthorized(ApiResponse<SupplierDTO>.FailureResponse("Invalid or missing token."));

                var supplier = await _supplierService.GetSupplierByIdAsync(userId);
                return Ok(ApiResponse<SupplierDTO>.SuccessResponse(supplier));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<SupplierDTO>.FailureResponse(ex.Message));
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<ApiResponse<IEnumerable<SupplierDTO>>>> GetSuppliers()
        {
            try
            {
                var suppliers = await _supplierService.GetAllSuppliersAsync();
                return Ok(ApiResponse<IEnumerable<SupplierDTO>>.SuccessResponse(suppliers));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<SupplierDTO>>.FailureResponse(ex.Message));
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Employee,Supplier")]
        public async Task<ActionResult<ApiResponse<SupplierDTO>>> GetSupplier(int id)
        {
            try
            {
                var supplier = await _supplierService.GetSupplierByIdAsync(id);
                return Ok(ApiResponse<SupplierDTO>.SuccessResponse(supplier));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<SupplierDTO>.FailureResponse(ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ApiResponse<SupplierDTO>>> PostSupplier(SupplierDTO supplier)
        {
            try
            {
                var createdSupplier = await _supplierService.CreateSupplierAsync(supplier);
                return CreatedAtAction(nameof(GetSupplier), new { id = createdSupplier.SupplierId }, ApiResponse<SupplierDTO>.SuccessResponse(createdSupplier));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<SupplierDTO>.FailureResponse(ex.Message));
            }
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin,Supplier")]
        public async Task<ActionResult<ApiResponse<string>>> PatchSupplier(int id, [FromBody] JsonPatchDocument<SupplierDTO> patchDoc)
        {
            try
            {
                if (patchDoc == null)
                    return BadRequest(ApiResponse<string>.FailureResponse("Invalid patch document."));

                await _supplierService.PatchSupplierAsync(id, patchDoc);
                return Ok(ApiResponse<string>.SuccessResponse("Supplier updated successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailureResponse(ex.Message));
            }
        }

        [HttpGet("{id}/pets")]
        [Authorize(Roles = "Admin,Employee,Supplier")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PetDto>>>> GetAllPets(int id)
        {
            try
            {
                var petDtos = await _supplierService.GetPetsAsync(id);
                return Ok(ApiResponse<IEnumerable<PetDto>>.SuccessResponse(petDtos));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<PetDto>>.FailureResponse(ex.Message));
            }
        }

        [HttpGet("{id}/pets/search")]
        [Authorize(Roles = "Admin,Employee,Supplier")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PetDto>>>> SearchPets(int id, [FromQuery] string query, [FromQuery] int? categoryId)
        {
            try
            {
                var petDtos = await _supplierService.SearchPetsAsync(id, query, categoryId);
                return Ok(ApiResponse<IEnumerable<PetDto>>.SuccessResponse(petDtos));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<PetDto>>.FailureResponse(ex.Message));
            }
        }

        [HttpPost("{id}/pets")]
        [Authorize(Roles = "Admin,Supplier")]
        public async Task<ActionResult<ApiResponse<PetDto>>> AddPet(int id, [FromBody] PetCreate dto)
        {
            try
            {
                var createdPet = await _supplierService.AddPetAsync(id, dto);
                return Ok(ApiResponse<PetDto>.SuccessResponse(createdPet));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PetDto>.FailureResponse(ex.Message));
            }
        }
    }
}
