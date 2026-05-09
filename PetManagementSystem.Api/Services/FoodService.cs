using AutoMapper;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Models;
using PetManagementSystem.Api.Repositories;

namespace PetManagementSystem.Api.Services
{
    public class FoodService : IFoodService
    {
        private readonly IFoodRepo _repo;
        private readonly IMapper _mapper;

        public FoodService(IFoodRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
       

        public async Task<IEnumerable<FoodDTO>> GetAllFoodsService()
        {
            var foods = await _repo.GetAllFoods();

            return _mapper.Map<IEnumerable<FoodDTO>>(foods);
        }

        public async  Task<FoodDTO?> GetFoodByIdService(int id)
        {
            var food = await _repo.GetFoodById(id);

            if (food == null)
            {
                return null;
            }

            return _mapper.Map<FoodDTO>(food);
        }

        public async Task<IEnumerable<FoodDTO>> GetFoodByPetIdService(int petId)
        {
            var foods = await _repo.GetFoodByPetId(petId);

            return _mapper.Map<IEnumerable<FoodDTO>>(foods);
        }

        public async Task<FoodDTO> AddFoodService(CreatePetFoodDto dto)
        {
            var food = _mapper.Map<PetFood>(dto);

            var result = await _repo.AddFood(food);

            return _mapper.Map<FoodDTO>(result);
        }
        public async Task<FoodDTO?> UpdateFoodService(int id, UpdatePetFoodDto dto)
        {
            var food = _mapper.Map<PetFood>(dto);

            var result = await _repo.UpdateFood(id, food);

            if (result == null)
            {
                return null;
            }

            return _mapper.Map<FoodDTO>(result);
        }
    }
}
