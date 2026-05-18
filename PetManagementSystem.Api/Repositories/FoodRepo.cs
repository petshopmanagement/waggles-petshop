using Microsoft.EntityFrameworkCore;
using PetManagementSystem.Api.Data;
using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Repositories
{
    public class FoodRepo : IFoodRepo
    {
        private readonly PetStoreDbContext _context;

        public FoodRepo(PetStoreDbContext context)
        {
            _context = context;
        }
       
        public async Task<IEnumerable<PetFood>> GetAllFoods()
        {
            return await _context.PetFoods.ToListAsync();
        }

        public async Task<PetFood?> GetFoodById(int id)
        {
            return await _context.PetFoods
                .FirstOrDefaultAsync(x => x.FoodId == id);
        }

        public async Task<IEnumerable<PetFood>> GetFoodByPetId(int petId)
        {
            var pet = await _context.Pets
               .Include(x => x.Foods)
               .FirstOrDefaultAsync(x => x.PetId == petId);

            if (pet == null)
            {
                return Enumerable.Empty<PetFood>();
            }

            return pet.Foods;
        }

        public async Task<PetFood> AddFood(PetFood food)
        {
            await _context.PetFoods.AddAsync(food);

            await _context.SaveChangesAsync();

            return food;
        }

        public async Task<PetFood?> UpdateFood(int id, PetFood food)
        {
            var existingFood = await _context.PetFoods
                .FirstOrDefaultAsync(x => x.FoodId == id);

            if (existingFood == null)
            {
                return null;
            }

            existingFood.Name = food.Name;
            existingFood.Brand = food.Brand;
            existingFood.Type = food.Type;
            existingFood.Quantity = food.Quantity;
            existingFood.Price = food.Price;

            await _context.SaveChangesAsync();

            return existingFood;
        }
    }
}
