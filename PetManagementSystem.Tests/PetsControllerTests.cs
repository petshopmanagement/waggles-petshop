using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
using PetManagementSystem.Api.Controllers;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Services;

namespace PetManagementSystem.Tests.Controllers
{
    public class PetsControllerTests
    {
        private readonly Mock<IPetService> _mockService;
        private readonly PetsController _controller;

        public PetsControllerTests()
        {
            _mockService = new Mock<IPetService>();
            _controller = new PetsController(_mockService.Object);
        }

        [Fact]
        public async Task GetAllPets_ReturnsOkResult_WithListOfPets()
        {
            // Arrange
            var pets = new List<PetDto> { new PetDto { PetId = 1, Name = "Buddy" } };
            _mockService.Setup(s => s.GetAllPets(1, 10)).ReturnsAsync(pets);

            // Act
            var result = await _controller.GetAllPets(1, 10);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(pets);
        }

        [Fact]
        public async Task GetPet_ReturnsOkResult_WithPet()
        {
            // Arrange
            var pet = new PetDto { PetId = 1, Name = "Buddy" };
            _mockService.Setup(s => s.GetPetById(1)).ReturnsAsync(pet);

            // Act
            var result = await _controller.GetPet(1);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(pet);
        }

        [Fact]
        public async Task PostPet_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var petCreateDto = new PetCreate { Name = "Buddy" };
            var createdPetDto = new PetDto { PetId = 1, Name = "Buddy" };
            _mockService.Setup(s => s.AddPet(petCreateDto)).ReturnsAsync(createdPetDto);

            // Act
            var result = await _controller.PostPet(petCreateDto);

            // Assert
            var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.ActionName.Should().Be(nameof(PetsController.GetPet));
            createdResult.RouteValues.Should().NotBeNull();
            createdResult.RouteValues!["petid"].Should().Be(1);
            createdResult.Value.Should().BeEquivalentTo(createdPetDto);
        }

        [Fact]
        public async Task PutPet_ReturnsNoContentResult()
        {
            // Arrange
            var petUpdateDto = new PetUpdate { Name = "Updated Buddy" };
            _mockService.Setup(s => s.UpdatePet(1, petUpdateDto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PutPet(1, petUpdateDto);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}

