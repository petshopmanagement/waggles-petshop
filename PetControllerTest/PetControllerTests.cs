using Microsoft.AspNetCore.Mvc;
using Moq;
using PetManagementSystem.Api.Controllers;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Services;

namespace PetManagementSystem.Tests.Controllers
{
    public class PetsControllerTests
    {
        private readonly Mock<IPetService> _serviceMock;
        private readonly PetsController _controller;

        public PetsControllerTests()
        {
            _serviceMock = new Mock<IPetService>();

            _controller = new PetsController(
                _serviceMock.Object);
        }

        // ====================================
        // POSITIVE TEST CASES
        // ====================================

        [Fact]
        public async Task GetAllPets_Returns_OkResult()
        {
            // Arrange
            var pets = new List<PetDTO>
            {
                new PetDTO
                {
                    PetId = 1,
                    Name = "Tommy"
                }
            };

            _serviceMock.Setup(s => s.GetAllPets())
                .ReturnsAsync(pets);

            // Act
            var result =
                await _controller.GetAllPets();

            // Assert
            var okResult =
                Assert.IsType<OkObjectResult>(result.Result);

            var returnedPets =
                Assert.IsAssignableFrom<IEnumerable<PetDTO>>(
                    okResult.Value);

            Assert.Single(returnedPets);
        }

        [Fact]
        public async Task GetPet_Returns_OkResult_WithPet()
        {
            // Arrange
            var pet = new PetDTO
            {
                PetId = 1,
                Name = "Rocky"
            };

            _serviceMock.Setup(s => s.GetPetById(1))
                .ReturnsAsync(pet);

            // Act
            var result =
                await _controller.GetPet(1);

            // Assert
            var okResult =
                Assert.IsType<OkObjectResult>(result.Result);

            var returnedPet =
                Assert.IsType<PetDTO>(okResult.Value);

            Assert.Equal("Rocky", returnedPet.Name);
        }

        [Fact]
        public async Task GetPetByCategory_Returns_Pets()
        {
            // Arrange
            var pets = new List<PetDTO>
            {
                new PetDTO
                {
                    PetId = 2,
                    Name = "Bruno"
                }
            };

            _serviceMock.Setup(s =>
                s.GetPetByCategory(1))
                .ReturnsAsync(pets);

            // Act
            var result =
                await _controller.GetPetByCategory(1);

            // Assert
            var okResult =
                Assert.IsType<OkObjectResult>(result.Result);

            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task PostPet_Returns_CreatedAtAction()
        {
            // Arrange
            var dto = new PetCreate
            {
                Name = "Kitty"
            };

            var createdPet = new PetDTO
            {
                PetId = 1,
                Name = "Kitty"
            };

            _serviceMock.Setup(s =>
                s.AddPet(dto))
                .ReturnsAsync(createdPet);

            // Act
            var result =
                await _controller.PostPet(dto);

            // Assert
            var createdResult =
                Assert.IsType<CreatedAtActionResult>(
                    result.Result);

            var returnedPet =
                Assert.IsType<PetDTO>(
                    createdResult.Value);

            Assert.Equal("Kitty", returnedPet.Name);
        }

        [Fact]
        public async Task PutPet_Returns_NoContent()
        {
            // Arrange
            var dto = new PetUpdate
            {
                Name = "Updated Pet"
            };

            _serviceMock.Setup(s =>
                s.UpdatePet(1, dto))
                .Returns(Task.CompletedTask);

            // Act
            var result =
                await _controller.PutPet(1, dto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        // ====================================
        // NEGATIVE TEST CASES
        // ====================================

        [Fact]
        public async Task GetPet_Returns_Exception_WhenPetNotFound()
        {
            // Arrange
            _serviceMock.Setup(s =>
                s.GetPetById(100))
                .ThrowsAsync(new Exception("Pet not found"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                () => _controller.GetPet(100));
        }

        [Fact]
        public async Task GetPetByCategory_Throws_Exception_WhenInvalidCategory()
        {
            // Arrange
            _serviceMock.Setup(s =>
                s.GetPetByCategory(0))
                .ThrowsAsync(new Exception("Invalid Category"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                () => _controller.GetPetByCategory(0));
        }

        [Fact]
        public async Task GetPetByName_Throws_Exception_WhenNameIsNull()
        {
            // Arrange
            _serviceMock.Setup(s =>
                s.GetPetByName(null))
                .ThrowsAsync(new Exception("Name required"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                () => _controller.GetPetByName(null));
        }

        [Fact]
        public async Task PostPet_Throws_Exception_WhenDtoInvalid()
        {
            // Arrange
            var dto = new PetCreate();

            _serviceMock.Setup(s =>
                s.AddPet(dto))
                .ThrowsAsync(new Exception("Invalid data"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                () => _controller.PostPet(dto));
        }

        [Fact]
        public async Task PutPet_Throws_Exception_WhenPetIdInvalid()
        {
            // Arrange
            var dto = new PetUpdate
            {
                Name = "Test"
            };

            _serviceMock.Setup(s =>
                s.UpdatePet(0, dto))
                .ThrowsAsync(new Exception("Invalid Pet Id"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(
                () => _controller.PutPet(0, dto));
        }
    }
}