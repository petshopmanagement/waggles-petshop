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
        public async Task<ActionResult<ApiResponse<IEnumerable<GroomingServiceDto>>>> GetAll()
        {
            var services = await _service.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<GroomingServiceDto>>.SuccessResponse(services));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GroomingServiceDto>>> GetById(int id)
        {
            var service = await _service.GetByIdAsync(id);
            return Ok(ApiResponse<GroomingServiceDto>.SuccessResponse(service));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<GroomingServiceDto>>> Create(GroomingServiceDto dto)
        {
            var createdService = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdService.ServiceId }, ApiResponse<GroomingServiceDto>.SuccessResponse(createdService));
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> Patch(int id, [FromBody] JsonPatchDocument<GroomingServiceDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest(ApiResponse<string>.FailureResponse("Invalid patch document."));

            await _service.PatchAsync(id, patchDoc);
            return Ok(ApiResponse<string>.SuccessResponse("Grooming service updated successfully."));
        }

        [HttpGet("{id}/pets")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PetDto>>>> GetPets(int id)
        {
            var pets = await _service.GetPetsAsync(id);
            return Ok(ApiResponse<IEnumerable<PetDto>>.SuccessResponse(pets));
        }
    }
}
