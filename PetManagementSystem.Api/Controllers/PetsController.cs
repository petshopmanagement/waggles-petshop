using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PetDto>>> GetAllPets([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pets = await _service.GetAllPets(page, pageSize);
                return Ok(pets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpGet("count")]
        [AllowAnonymous]
        public async Task<ActionResult<int>> GetTotalPetsCount()
        {
            try
            {
                var count = await _service.GetTotalPetCount();
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpGet("{petid}")]
        [AllowAnonymous]
        public async Task<ActionResult<PetDto>> GetPet(int petid)
        {
            try
            {
                var pet = await _service.GetPetById(petid);
                return Ok(pet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpGet("category/{categoryId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PetDto>>> GetPetByCategory(int categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pets = await _service.GetPetByCategory(categoryId, page, pageSize);
                return Ok(pets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpGet("name/{name}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PetDto>>> GetPetByName(string name, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var pets = await _service.GetPetByName(name, page, pageSize);
                return Ok(pets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Supplier,Admin")]
        public async Task<ActionResult<PetDto>> PostPet([FromBody] PetCreate dto)
        {
            try
            {
                var createdPet = await _service.AddPet(dto);

                return CreatedAtAction(
                    nameof(GetPet),
                    new { petid = createdPet.PetId },
                    createdPet
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpPut("{petid}")]
        [Authorize(Roles = "Supplier,Admin")]
        public async Task<IActionResult> PutPet(int petid, [FromBody] PetUpdate dto)
        {
            try
            {
                await _service.UpdatePet(petid, dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpGet("{petId}/suppliers")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<IEnumerable<SupplierDTO>>> GetSupplierPetById(int petId)
        {
            try
            {
                var suppliers = await _service.GetSuppliersByPetIdService(petId);
                return Ok(suppliers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpGet("{petId}/employees")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeeByPetId(int petId)
        {
            try
            {
                var employees = await _service.GetEmployeeByPetIdService(petId);
                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpGet("transactions/{petId}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactionsByPetId(int petId)
        {
            try
            {
                var result = await _service.GetTrasactionbypetID(petId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpGet("groomings/{petId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<GroomingDTO>>> GetGroomingsByPetId(int petId)
        {
            try
            {
                var result = await _service.GetGroomingsByPetId(petId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpGet("vaccinations/{petId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<VaccinationDto>>> GetVaccinationByPetId(int petId)
        {
            try
            {
                var result = await _service.GetVaccinationByPetId(petId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

    }
}