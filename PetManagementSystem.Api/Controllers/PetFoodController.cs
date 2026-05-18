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
        public async Task<IActionResult>
            GetAllFoods()
        {
            return Ok(
                await _service.GetAllFoodsService());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult>GetFoodById(int id)
        {
            return Ok(
                await _service.GetFoodByIdService(id));
        }

        [HttpGet("pet/{petId}")]
        public async Task<IActionResult>
            GetFoodByPetId(int petId)
        {
            return Ok(
                await _service.GetFoodByPetIdService(petId));
        }

        [HttpPost]
        public async Task<IActionResult>
            AddFood(CreatePetFoodDto dto)
        {
            return Ok(
                await _service.AddFoodService(dto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult>
            UpdateFood(
                int id,
                UpdatePetFoodDto dto)
        {
            return Ok(
                await _service.UpdateFoodService(id, dto));
        }
    }
}