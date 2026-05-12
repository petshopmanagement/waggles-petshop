using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using PetManagementSystem.Api.Controllers;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Helpers;
using PetManagementSystem.Api.Services;
using PetManagementSystem.Api.Exceptions;
using Xunit;

namespace PetManagementSystem.Tests
{
    public class GroomingServicesControllerTests
    {
        private readonly Mock<IGroomingServiceService> _mockService;
        private readonly GroomingServicesController _controller;

        public GroomingServicesControllerTests()
        {
            _mockService = new Mock<IGroomingServiceService>();
            _controller = new GroomingServicesController(_mockService.Object);
        }

       
        [Fact]
        public async Task GetAll_Positive_ReturnsOk()
        {
            // Arrange
            var services = new List<GroomingServiceDto> { new GroomingServiceDto { ServiceId = 1, Name = "Wash" } };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(services);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<ApiResponse<IEnumerable<GroomingServiceDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task GetAll_Negative_ThrowsException()
        {
            // Arrange
            _mockService.Setup(s => s.GetAllAsync()).ThrowsAsync(new DataNotFoundException("No grooming services found."));

            // Act & Assert
            await Assert.ThrowsAsync<DataNotFoundException>(() => _controller.GetAll());
        }

        
        [Fact]
        public async Task GetById_Positive_ReturnsOk()
        {
            // Arrange
            var service = new GroomingServiceDto { ServiceId = 1, Name = "Wash" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(service);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<ApiResponse<GroomingServiceDto>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(1, response.Data.ServiceId);
        }

        [Fact]
        public async Task GetById_Negative_ThrowsException()
        {
            // Arrange
            _mockService.Setup(s => s.GetByIdAsync(1)).ThrowsAsync(new DataNotFoundException("Grooming service not found."));

            // Act & Assert
            await Assert.ThrowsAsync<DataNotFoundException>(() => _controller.GetById(1));
        }

       
        [Fact]
        public async Task Create_Positive_ReturnsCreated()
        {
            // Arrange
            var createDto = new CreateGroomingServiceDto { Name = "Wash" };
            var createdDto = new GroomingServiceDto { ServiceId = 1, Name = "Wash" };
            _mockService.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(createdDto);

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var response = Assert.IsType<ApiResponse<GroomingServiceDto>>(createdResult.Value);
            Assert.True(response.Success);
            Assert.Equal(1, response.Data.ServiceId);
        }

        [Fact]
        public async Task Create_Negative_ThrowsException()
        {
            // Arrange
            var createDto = new CreateGroomingServiceDto { Name = "Wash" };
            _mockService.Setup(s => s.CreateAsync(createDto)).ThrowsAsync(new Exception("Creation failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _controller.Create(createDto));
        }

       
        [Fact]
        public async Task Patch_Positive_ReturnsOk()
        {
            // Arrange
            var patchDoc = new JsonPatchDocument<UpdateGroomingServiceDto>();
            _mockService.Setup(s => s.PatchAsync(1, patchDoc)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Patch(1, patchDoc);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<ApiResponse<string>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("Grooming service updated successfully.", response.Data);
        }

        [Fact]
        public async Task Patch_Negative_ReturnsBadRequest_WhenDocIsNull()
        {
            // Act
            var result = await _controller.Patch(1, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var response = Assert.IsType<ApiResponse<string>>(badRequestResult.Value);
            Assert.False(response.Success);
            Assert.Equal("Invalid patch document.", response.Message);
        }

        
        [Fact]
        public async Task GetPets_Positive_ReturnsOk()
        {
            // Arrange
            var pets = new List<PetDto> { new PetDto { PetId = 1, Name = "Buddy" } };
            _mockService.Setup(s => s.GetPetsAsync(1)).ReturnsAsync(pets);

            // Act
            var result = await _controller.GetPets(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<ApiResponse<IEnumerable<PetDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task GetPets_Negative_ThrowsException()
        {
            // Arrange
            _mockService.Setup(s => s.GetPetsAsync(1)).ThrowsAsync(new DataNotFoundException("No pets found."));

            // Act & Assert
            await Assert.ThrowsAsync<DataNotFoundException>(() => _controller.GetPets(1));
        }
    }
}
