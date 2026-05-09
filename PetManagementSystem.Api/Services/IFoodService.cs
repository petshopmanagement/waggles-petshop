using PetManagementSystem.Api.DTOs;

namespace PetManagementSystem.Api.Services
{
    public interface IFoodService
    {
        Task<IEnumerable<FoodDTO>> GetAllFoodsService();

        Task<FoodDTO?> GetFoodByIdService(int id);

        Task<IEnumerable<FoodDTO>> GetFoodByPetIdService(int petId);

        Task<FoodDTO> AddFoodService(CreatePetFoodDto dto);

        Task<FoodDTO?> UpdateFoodService(int id, UpdatePetFoodDto dto);
    }
}
