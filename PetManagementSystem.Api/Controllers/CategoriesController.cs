using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetManagementSystem.Api.Data;
using PetManagementSystem.Api.Helpers;
using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly PetStoreDbContext _context;

        public CategoriesController(PetStoreDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<object>>>> GetCategories()
        {
            var categories = await _context.PetCategories
                .Select(c => new { c.CategoryId, c.Name })
                .ToListAsync();
            return Ok(ApiResponse<IEnumerable<object>>.SuccessResponse(categories));
        }
    }
}
