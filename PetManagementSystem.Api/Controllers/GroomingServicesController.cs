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
    [Authorize]
    public class GroomingServicesController : ControllerBase
    {
        private readonly IGroomingServiceService _service;

        public GroomingServicesController(IGroomingServiceService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<IEnumerable<GroomingDTO>>>> GetAll()
        {
            try
            {
                var services = await _service.GetAllAsync();
                return Ok(ApiResponse<IEnumerable<GroomingDTO>>.SuccessResponse(services));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<GroomingDTO>>.FailureResponse(ex.Message));
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<GroomingDTO>>> GetById(int id)
        {
            try
            {
                var service = await _service.GetByIdAsync(id);
                return Ok(ApiResponse<GroomingDTO>.SuccessResponse(service));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<GroomingDTO>.FailureResponse(ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<ApiResponse<GroomingDTO>>> Create(GroomingDTO dto)
        {
            try
            {
                var createdService = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = createdService.ServiceId }, ApiResponse<GroomingDTO>.SuccessResponse(createdService));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<GroomingDTO>.FailureResponse(ex.Message));
            }
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<ApiResponse<string>>> Patch(int id, [FromBody] JsonPatchDocument<GroomingDTO> patchDoc)
        {
            try
            {
                if (patchDoc == null)
                    return BadRequest(ApiResponse<string>.FailureResponse("Invalid patch document."));

                await _service.PatchAsync(id, patchDoc);
                return Ok(ApiResponse<string>.SuccessResponse("Grooming service updated successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailureResponse(ex.Message));
            }
        }

        [HttpGet("{id}/pets")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PetDto>>>> GetPets(int id)
        {
            try
            {
                var pets = await _service.GetPetsAsync(id);
                return Ok(ApiResponse<IEnumerable<PetDto>>.SuccessResponse(pets));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<PetDto>>.FailureResponse(ex.Message));
            }
        }
    }
}
