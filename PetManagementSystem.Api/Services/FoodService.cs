using AutoMapper;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Exceptions;
using PetManagementSystem.Api.Models;
using PetManagementSystem.Api.Repositories;

namespace PetManagementSystem.Api.Services
{
    public class FoodService : IFoodService
    {
        private readonly IFoodRepo _repo;
        private readonly IMapper _mapper;

        public FoodService(
            IFoodRepo repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FoodDTO>>
            GetAllFoodsService()
        {
            var foods = await _repo.GetAllFoods();

            if (foods == null || !foods.Any())
            {
                throw new ResourceNotFoundException(
                    "Food",
                    "All");
            }

            return _mapper.Map<IEnumerable<FoodDTO>>(foods);
        }

        public async Task<FoodDTO?>
            GetFoodByIdService(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException(
                    "Food Id must be greater than zero");
            }

            var food = await _repo.GetFoodById(id);

            if (food == null)
            {
                throw new ResourceNotFoundException(
                    "Food Id",
                    id);
            }

            return _mapper.Map<FoodDTO>(food);
        }

        public async Task<IEnumerable<FoodDTO>>
            GetFoodByPetIdService(int petId)
        {
            if (petId <= 0)
            {
                throw new ValidationException(
                    "Pet Id is invalid");
            }

            var foods =
                await _repo.GetFoodByPetId(petId);

            if (!foods.Any())
            {
                throw new ResourceNotFoundException(
                    "Pet Food",
                    petId);
            }

            return _mapper.Map<IEnumerable<FoodDTO>>(foods);
        }

        public async Task<FoodDTO>
            AddFoodService(CreatePetFoodDto dto)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                errors.Add("Food Name is required");
            }

            if (string.IsNullOrWhiteSpace(dto.Brand))
            {
                errors.Add("Brand is required");
            }

            if (dto.Price <= 0)
            {
                errors.Add(
                    "Price must be greater than zero");
            }

            if (dto.Quantity < 0)
            {
                errors.Add(
                    "Quantity cannot be negative");
            }

            if (errors.Any())
            {
                throw new ValidationException(errors);
            }

            var food =
                _mapper.Map<PetFood>(dto);

            var result =
                await _repo.AddFood(food);

            return _mapper.Map<FoodDTO>(result);
        }

        public async Task<FoodDTO?>
            UpdateFoodService(
                int id,
                UpdatePetFoodDto dto)
        {
            if (id <= 0)
            {
                throw new ValidationException(
                    "Invalid Food Id");
            }

            var existingFood =
                await _repo.GetFoodById(id);

            if (existingFood == null)
            {
                throw new ResourceNotFoundException(
                    "Food",
                    id);
            }

            var food =
                _mapper.Map<PetFood>(dto);

            var result =
                await _repo.UpdateFood(id, food);

            return _mapper.Map<FoodDTO>(result);
        }
    }
}