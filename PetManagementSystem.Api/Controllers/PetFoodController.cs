using Microsoft.AspNetCore.Mvc;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Services;

namespace PetManagementSystem.Api.Controllers
{
    [Route("api/PetFood")]
    [ApiController]
    public class PetFoodController : ControllerBase
    {
        private readonly IFoodService _service;

        public PetFoodController(
            IFoodService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFoods()
        {
            try
            {
                return Ok(await _service.GetAllFoodsService());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFoodById(int id)
        {
            try
            {
                return Ok(await _service.GetFoodByIdService(id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpGet("pet/{petId}")]
        public async Task<IActionResult> GetFoodByPetId(int petId)
        {
            try
            {
                return Ok(await _service.GetFoodByPetIdService(petId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddFood(CreatePetFoodDto dto)
        {
            try
            {
                return Ok(await _service.AddFoodService(dto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFood(int id, UpdatePetFoodDto dto)
        {
            try
            {
                return Ok(await _service.UpdateFoodService(id, dto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }
    }
}