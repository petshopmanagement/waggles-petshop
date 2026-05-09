using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Services;

namespace PetManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeFoodController : ControllerBase
    {
        private readonly IFoodService _service;

        public PeFoodController(IFoodService service)
        {
            _service = service;
        }

        // GET: api/petfood
        [HttpGet]
        public async Task<IActionResult> GetAllFoods()
        {
            var result = await _service.GetAllFoodsService();

            return Ok(result);
        }

        // GET: api/petfood/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFoodById(int id)
        {
            var result = await _service.GetFoodByIdService(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // GET: api/petfood/pet/{petId}
        [HttpGet("pet/{petId}")]
        public async Task<IActionResult> GetFoodByPetId(int petId)
        {
            var result = await _service.GetFoodByPetIdService(petId);

            return Ok(result);
        }

        // POST: api/petfood
        [HttpPost]
        public async Task<IActionResult> AddFood(CreatePetFoodDto dto)
        {
            var result = await _service.AddFoodService(dto);

            return Ok(result);
        }

        // PUT: api/petfood/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFood(int id, UpdatePetFoodDto dto)
        {
            var result = await _service.UpdateFoodService(id, dto);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
