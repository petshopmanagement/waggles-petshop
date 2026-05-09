using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetManagementSystem.Api.Data;
using PetManagementSystem.Api.Models;
using PetManagementSystem.Api.Services;
using PetManagementSystem.Api.DTOs;

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
    public async Task<ActionResult<IEnumerable<Pet>>> Pet()
    {
        var pets = await _service.GetAllPets();

        return Ok(pets);
    }


    [HttpGet("{petid}")]
    public async Task<ActionResult<Pet>> GetPet(int petid)
    {
        var pet = await _service.GetPetById(petid);

        if (pet == null)
        {
            return NotFound(new
            {
                message = "Pet not found"
            });
        }

        return Ok(pet);
    }

    [HttpGet("category/{category_id}")]
    public async Task<ActionResult<IEnumerable<Pet>>> GetPetbyCategory(int category_id)
    {
        var pets = await _service.GetPetByCategory(category_id);

        if (!pets.Any())
        {
            return NotFound(new
            {
                message = "No pets found in this category"
            });
        }
        return Ok(pets);
    }

    //searching pets by their name
    [HttpGet("Name/{name}")]
    public async Task<ActionResult<IEnumerable<Pet>>> GetPetbyname(string name)
    {
        var pets = await _service.GetPetByName(name);

        if (!pets.Any())
        {
            return NotFound(new
            {
                message = "No pets found with this name"
            });
        }

        return Ok(pets);
    }

    [HttpPut("{petid}")]
    public async Task<IActionResult> PutPet(int petid, PetUpdate dto)
    {
        if (petid != dto.PetId)
        {
            return BadRequest(new
            {
                message = "Pet ID mismatch"
            });
        }

        await _service.UpdatePet(petid, dto);

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<PetDTO>> PostPet(PetCreate dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdPet = await _service.AddPet(dto);

        return CreatedAtAction(
            nameof(GetPet),
            new { petid = createdPet.Name },
            createdPet
        );
    }

    //[HttpGet("{petId}/suppliers")]
    //public async Task<ActionResult<IEnumerable<SupplierDTO>>> GetSuppliersByPetId(int petId)
    //{
    //    var suppliers = await _service.GetSuppliersByPetIdService(petId);
    //    if (suppliers == null || !suppliers.Any())
    //    {
    //        return NotFound(new
    //        {
    //            message = "No suppliers found for this pet"
    //        });
    //    }
    //    return Ok(suppliers);
    //}

    [HttpGet("{petId}/suppliers")]
    public async Task<ActionResult<IEnumerable<SupplierDTO>>> GetSupplierPetById(int petId)
    {
        var suppliers = await _service.GetSuppliersByPetIdService(petId);

        if (!suppliers.Any())
        {
            return NotFound(new
            {
                message = "No suppliers found for this pet"
            });
        }

        return Ok(suppliers);
    }
    [HttpGet("{petId}/employees")]
    public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployeeByPetId(int petId)
    {
        var employees = await _service.GetEmployeeByPetIdService(petId);

        if (!employees.Any())
        {
            return NotFound(new
            {
                message = "No employees found for this pet"
            });
        }

        return Ok(employees);
    }

    [HttpGet("transactions/{petId}")]
    public async Task<IActionResult> GetTransactionsByPetId(int petId)
    {
        var result = await _service.GetTrasactionbypetID(petId);

        return Ok(result);
    }

    [HttpGet("groomings/{petId}")]
    public async Task<IActionResult> GetGroomingsByPetId(int petId)
    {
        var result = await _service.GetGroomingsByPetId(petId);

        return Ok(result);
    }

    // GET: api/pets/vaccinations/1
    [HttpGet("vaccinations/{petId}")]
    public async Task<IActionResult> GetVaccinationByPetId(int petId)
    {
        var result = await _service.GetVaccinationByPetId(petId);

        return Ok(result);
    }
}