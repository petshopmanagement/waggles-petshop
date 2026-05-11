using Microsoft.AspNetCore.Mvc;
using Moq;
using PetManagementSystem.Api.Controllers;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Services;

namespace PetManagementSystem.Tests.Controllers
{
    public class PeFoodControllerTests
    {
        private readonly Mock<IFoodService> _serviceMock;
        private readonly PeFoodController _controller;

        public PeFoodControllerTests()
        {
            _serviceMock = new Mock<IFoodService>();

            _controller = new PeFoodController(
                _serviceMock.Object);
        }

        // ====================================
        // POSITIVE TEST CASES
        // ====================================

        [Fact]
        public async Task GetAllFoods_Returns_OkResult()
        {
            // Arrange
            var foods = new List<FoodDTO>
            {
                new FoodDTO
                {
                    FoodId = 1,
                    Name = "Pedigree"
                }
            };

            _serviceMock.Setup(s =>
                s.GetAllFoodsService())
                .ReturnsAsync(foods);

            // Act
            var result =
                await _controller.GetAllFoods();

            // Assert
            var okResult =
                Assert.IsType<OkObjectResult>(result);

            var returnedFoods =
                Assert.IsAssignableFrom<IEnumerable<FoodDTO>>(
                    okResult.Value);

            Assert.Single(returnedFoods);
        }

        [Fact]
        public async Task GetFoodById_Returns_OkResult_WithFood()
        {
            // Arrange
            var food = new FoodDTO
            {
                FoodId = 1,
                Name = "Royal Canin"
            };

            _serviceMock.Setup(s =>
                s.GetFoodByIdService(1))
                .ReturnsAsync(food);

            // Act
            var result =
                await _controller.GetFoodById(1);

            // Assert
            var okResult =
                Assert.IsType<OkObjectResult>(result);

            var returnedFood =
                Assert.IsType<FoodDTO>(
                    okResult.Value);

            Assert.Equal("Royal Canin",
                returnedFood.Name);
        }

        [Fact]
        public async Task GetFoodByPetId_Returns_FoodList()
        {
            // Arrange
            var foods = new List<FoodDTO>
            {
                new FoodDTO
                {
                    FoodId = 1,
                    Name = "Whiskas"
                }
            };

            _serviceMock.Setup(s =>
                s.GetFoodByPetIdService(1))
                .ReturnsAsync(foods);

            // Act
            var result =
                await _controller.GetFoodByPetId(1);

            // Assert
            var okResult =
                Assert.IsType<OkObjectResult>(result);

            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task AddFood_Returns_OkResult()
        {
            // Arrange
            var dto = new CreatePetFoodDto
            {
                Name = "Drools",
                Brand = "Drools Brand",
                Price = 500,
                Quantity = 2
            };

            var foodDto = new FoodDTO
            {
                FoodId = 1,
                Name = "Drools"
            };

            _serviceMock.Setup(s =>
                s.AddFoodService(dto))
                .ReturnsAsync(foodDto);

            // Act
            var result =
                await _controller.AddFood(dto);

            // Assert
            var okResult =
                Assert.IsType<OkObjectResult>(result);

            var returnedFood =
                Assert.IsType<FoodDTO>(
                    okResult.Value);

            Assert.Equal("Drools",
                returnedFood.Name);
        }

        [Fact]
        public async Task UpdateFood_Returns_OkResult()
        {
            // Arrange
            var dto = new UpdatePetFoodDto
            {
                Name = "Updated Food"
            };

            var updatedFood = new FoodDTO
            {
                FoodId = 1,
                Name = "Updated Food"
            };

            _serviceMock.Setup(s =>
                s.UpdateFoodService(1, dto))
                .ReturnsAsync(updatedFood);

            // Act
            var result =
                await _controller.UpdateFood(1, dto);

            // Assert
            var okResult =
                Assert.IsType<OkObjectResult>(result);

            var returnedFood =
                Assert.IsType<FoodDTO>(
                    okResult.Value);

            Assert.Equal("Updated Food",
                returnedFood.Name);
        }

        // ====================================
        // NEGATIVE TEST CASES
        // ====================================

        [Fact]
        public async Task GetAllFoods_Throws_Exception_WhenNoFoodsFound()
        {
            // Arrange
            _serviceMock.Setup(s =>
                s.GetAllFoodsService())
                .ThrowsAsync(new Exception("No foods found"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                () => _controller.GetAllFoods());
        }

        [Fact]
        public async Task GetFoodById_Throws_Exception_WhenFoodNotFound()
        {
            // Arrange
            _serviceMock.Setup(s =>
                s.GetFoodByIdService(100))
                .ThrowsAsync(new Exception("Food not found"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                () => _controller.GetFoodById(100));
        }

        [Fact]
        public async Task GetFoodByPetId_Throws_Exception_WhenPetIdInvalid()
        {
            // Arrange
            _serviceMock.Setup(s =>
                s.GetFoodByPetIdService(0))
                .ThrowsAsync(new Exception("Invalid Pet Id"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                () => _controller.GetFoodByPetId(0));
        }

        [Fact]
        public async Task AddFood_Throws_Exception_WhenDtoInvalid()
        {
            // Arrange
            var dto = new CreatePetFoodDto();

            _serviceMock.Setup(s =>
                s.AddFoodService(dto))
                .ThrowsAsync(new Exception("Invalid Food Data"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                () => _controller.AddFood(dto));
        }

        [Fact]
        public async Task UpdateFood_Throws_Exception_WhenIdInvalid()
        {
            // Arrange
            var dto = new UpdatePetFoodDto
            {
                Name = "Test Food"
            };

            _serviceMock.Setup(s =>
                s.UpdateFoodService(0, dto))
                .ThrowsAsync(new Exception("Invalid Food Id"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                () => _controller.UpdateFood(0, dto));
        }
    }
}