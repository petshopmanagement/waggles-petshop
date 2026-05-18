using Microsoft.AspNetCore.Mvc;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Services;

namespace PetManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly IPetService _service;

        public PetsController(IPetService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetDto>>> GetAllPets([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var pets = await _service.GetAllPets(page, pageSize);
            return Ok(pets);
        }

        [HttpGet("{petid}")]
        public async Task<ActionResult<PetDto>> GetPet(int petid)
        {
            var pet = await _service.GetPetById(petid);
            return Ok(pet);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<PetDto>>> GetPetByCategory(int categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var pets = await _service.GetPetByCategory(categoryId, page, pageSize);
            return Ok(pets);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<IEnumerable<PetDto>>> GetPetByName(string name, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var pets = await _service.GetPetByName(name, page, pageSize);
            return Ok(pets);
        }

        [HttpPost]
        public async Task<ActionResult<PetDto>> PostPet([FromBody] PetCreate dto)
        {
            var createdPet = await _service.AddPet(dto);

            return CreatedAtAction(
                nameof(GetPet),
                new { petid = createdPet.PetId },
                createdPet
            );
        }

        [HttpPut("{petid}")]
        public async Task<IActionResult> PutPet(int petid, [FromBody] PetUpdate dto)
        {
            await _service.UpdatePet(petid, dto);
            return NoContent();
        }

        [HttpGet("{petId}/suppliers")]
        public async Task<ActionResult<IEnumerable<SupplierDTO>>> GetSupplierPetById(int petId)
        {
            var suppliers = await _service.GetSuppliersByPetIdService(petId);
            return Ok(suppliers);
        }

        [HttpGet("{petId}/employees")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeeByPetId(int petId)
        {
            var employees = await _service.GetEmployeeByPetIdService(petId);
            return Ok(employees);
        }

        [HttpGet("transactions/{petId}")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactionsByPetId(int petId)
        {
            var result = await _service.GetTrasactionbypetID(petId);
            return Ok(result);
        }

        [HttpGet("groomings/{petId}")]
        public async Task<ActionResult<IEnumerable<GroomingDTO>>> GetGroomingsByPetId(int petId)
        {
            var result = await _service.GetGroomingsByPetId(petId);
            return Ok(result);
        }

        [HttpGet("vaccinations/{petId}")]
        public async Task<ActionResult<IEnumerable<VaccinationDto>>> GetVaccinationByPetId(int petId)
        {
            var result = await _service.GetVaccinationByPetId(petId);
            return Ok(result);
        }
    }
}