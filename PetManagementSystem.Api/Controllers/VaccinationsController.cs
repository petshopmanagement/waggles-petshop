using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Services;

namespace PetManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VaccinationsController : ControllerBase
    {
        private readonly IVaccinationService _service;

        public VaccinationsController(IVaccinationService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var vaccinations = await _service.GetAllAsync();
                return Ok(vaccinations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var vaccination = await _service.GetByIdAsync(id);
                return Ok(vaccination);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Create(WriteVaccinationDto dto)
        {
            try
            {
                var createdVaccination = await _service.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = createdVaccination.VaccinationId },
                    createdVaccination);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Update(int id, WriteVaccinationDto dto)
        {
            try
            {
                var updatedVaccination = await _service.UpdateAsync(id, dto);
                return Ok(updatedVaccination);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Patch(int id, WriteVaccinationDto dto)
        {
            try
            {
                var updatedVaccination = await _service.PatchAsync(id, dto);
                return updatedVaccination == null ? NotFound() : Ok(updatedVaccination);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpGet("{id}/pets")]
        public async Task<IActionResult> GetPets(int id)
        {
            try
            {
                var pets = await _service.GetPetsAsync(id);
                return Ok(pets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }
    }
}