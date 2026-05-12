using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Services;

namespace PetManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccinationsController : ControllerBase
    {
        private readonly IVaccinationService _service;

        public VaccinationsController(IVaccinationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vaccinations = await _service.GetAllAsync();
            return Ok(vaccinations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var vaccination = await _service.GetByIdAsync(id);
            return Ok(vaccination);
        }

        [HttpPost]
        public async Task<IActionResult> Create(WriteVaccinationDto dto)
        {
            var createdVaccination = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdVaccination.VaccinationId },
                createdVaccination);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, WriteVaccinationDto dto)
        {
            var updatedVaccination = await _service.UpdateAsync(id, dto);
            return Ok(updatedVaccination);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, WriteVaccinationDto dto)
        {
            var updatedVaccination = await _service.PatchAsync(id, dto);
            return updatedVaccination == null ? NotFound() : Ok(updatedVaccination);
        }

        [HttpGet("{id}/pets")]
        public async Task<IActionResult> GetPets(int id)
        {
            var pets = await _service.GetPetsAsync(id);
            return Ok(pets);
        }
    }
}