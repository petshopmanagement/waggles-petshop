
using AutoMapper;
using Moq;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Exceptions;
using PetManagementSystem.Api.Models;
using PetManagementSystem.Api.Repositories;
using PetManagementSystem.Api.Services;

namespace PetManagementSystem.Tests.Services
{
    public class UnitTest1
    {
        private readonly Mock<IFoodRepo> _repoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly FoodService _service;

        public UnitTest1()
        {
            _repoMock = new Mock<IFoodRepo>();
            _mapperMock = new Mock<IMapper>();

            _service = new FoodService(
                _repoMock.Object,
                _mapperMock.Object);
        }

        // =========================
        // POSITIVE TEST CASES
        // =========================

        [Fact]
        public async Task GetAllFoodsService_Returns_AllFoods()
        {
            // Arrange
            var foods = new List<PetFood>
            {
                new PetFood { FoodId = 1, Name = "Dog Food" }
            };

            var foodDtos = new List<FoodDTO>
            {
                new FoodDTO { FoodId = 1, Name = "Dog Food" }
            };

            _repoMock.Setup(r => r.GetAllFoods())
                .ReturnsAsync(foods);

            _mapperMock.Setup(m =>
                m.Map<IEnumerable<FoodDTO>>(foods))
                .Returns(foodDtos);

            // Act
            var result =
                await _service.GetAllFoodsService();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GetFoodByIdService_Returns_Food_WhenValidId()
        {
            // Arrange
            var food = new PetFood
            {
                FoodId = 1,
                Name = "Cat Food"
            };

            var foodDto = new FoodDTO
            {
                FoodId = 1,
                Name = "Cat Food"
            };

            _repoMock.Setup(r => r.GetFoodById(1))
                .ReturnsAsync(food);

            _mapperMock.Setup(m =>
                m.Map<FoodDTO>(food))
                .Returns(foodDto);

            // Act
            var result =
                await _service.GetFoodByIdService(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Cat Food", result.Name);
        }

        [Fact]
        public async Task GetFoodByPetIdService_Returns_Foods()
        {
            // Arrange
            var foods = new List<PetFood>
            {
                new PetFood { FoodId = 1, Name = "Fish Food" }
            };

            var foodDtos = new List<FoodDTO>
            {
                new FoodDTO { FoodId = 1, Name = "Fish Food" }
            };

            _repoMock.Setup(r => r.GetFoodByPetId(1))
                .ReturnsAsync(foods);

            _mapperMock.Setup(m =>
                m.Map<IEnumerable<FoodDTO>>(foods))
                .Returns(foodDtos);

            // Act
            var result =
                await _service.GetFoodByPetIdService(1);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task AddFoodService_Adds_Food_Successfully()
        {
            // Arrange
            var dto = new CreatePetFoodDto
            {
                Name = "Pedigree",
                Brand = "Mars",
                Price = 200,
                Quantity = 5
            };

            var food = new PetFood
            {
                Name = dto.Name
            };

            var addedFood = new PetFood
            {
                FoodId = 1,
                Name = dto.Name
            };

            var foodDto = new FoodDTO
            {
                FoodId = 1,
                Name = dto.Name
            };

            _mapperMock.Setup(m =>
                m.Map<PetFood>(dto))
                .Returns(food);

            _repoMock.Setup(r =>
                r.AddFood(food))
                .ReturnsAsync(addedFood);

            _mapperMock.Setup(m =>
                m.Map<FoodDTO>(addedFood))
                .Returns(foodDto);

            // Act
            var result =
                await _service.AddFoodService(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Pedigree", result.Name);
        }

        [Fact]
        public async Task UpdateFoodService_Updates_Food_Successfully()
        {
            // Arrange
            var dto = new UpdatePetFoodDto
            {
                Name = "Updated Food"
            };

            var existingFood = new PetFood
            {
                FoodId = 1,
                Name = "Old Food"
            };

            var updatedFood = new PetFood
            {
                FoodId = 1,
                Name = "Updated Food"
            };

            var foodDto = new FoodDTO
            {
                FoodId = 1,
                Name = "Updated Food"
            };

            _repoMock.Setup(r => r.GetFoodById(1))
                .ReturnsAsync(existingFood);

            _mapperMock.Setup(m =>
                m.Map<PetFood>(dto))
                .Returns(updatedFood);

            _repoMock.Setup(r =>
                r.UpdateFood(1, updatedFood))
                .ReturnsAsync(updatedFood);

            _mapperMock.Setup(m =>
                m.Map<FoodDTO>(updatedFood))
                .Returns(foodDto);

            // Act
            var result =
                await _service.UpdateFoodService(1, dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Food", result.Name);
        }

        // =========================
        // NEGATIVE TEST CASES
        // =========================

        [Fact]
        public async Task GetAllFoodsService_Throws_Exception_WhenNoFoods()
        {
            // Arrange
            _repoMock.Setup(r => r.GetAllFoods())
                .ReturnsAsync(new List<PetFood>());

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(
                () => _service.GetAllFoodsService());
        }

        [Fact]
        public async Task GetFoodByIdService_Throws_Exception_WhenIdInvalid()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(
                () => _service.GetFoodByIdService(0));
        }

        [Fact]
        public async Task GetFoodByIdService_Throws_Exception_WhenFoodNotFound()
        {
            // Arrange
            _repoMock.Setup(r => r.GetFoodById(10))
                .ReturnsAsync((PetFood)null);

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(
                () => _service.GetFoodByIdService(10));
        }

        [Fact]
        public async Task AddFoodService_Throws_Exception_WhenNameIsMissing()
        {
            // Arrange
            var dto = new CreatePetFoodDto
            {
                Name = "",
                Brand = "Mars",
                Price = 100,
                Quantity = 2
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(
                () => _service.AddFoodService(dto));
        }

        [Fact]
        public async Task UpdateFoodService_Throws_Exception_WhenFoodNotFound()
        {
            // Arrange
            var dto = new UpdatePetFoodDto
            {
                Name = "Updated"
            };

            _repoMock.Setup(r => r.GetFoodById(5))
                .ReturnsAsync((PetFood)null);

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(
                () => _service.UpdateFoodService(5, dto));
        }
    }
}

