using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Repositories
{
    public interface IFoodRepo
    {
        Task<IEnumerable<PetFood>> GetAllFoods();

        Task<PetFood?> GetFoodById(int id);

        Task<IEnumerable<PetFood>> GetFoodByPetId(int petId);

        Task<PetFood> AddFood(PetFood food);

        Task<PetFood?> UpdateFood(int id, PetFood food);
    }
}
