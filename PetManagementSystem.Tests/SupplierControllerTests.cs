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
    public class SupplierControllerTests
    {
        private readonly Mock<ISupplierService> _mockService;
        private readonly SupplierController _controller;

        public SupplierControllerTests()
        {
            _mockService = new Mock<ISupplierService>();
            _controller = new SupplierController(_mockService.Object);
        }

        // 1. GetSuppliers
        [Fact]
        public async Task GetSuppliers_Positive_ReturnsOk()
        {
            // Arrange
            var suppliers = new List<SupplierDto> { new SupplierDto { SupplierId = 1, Name = "Test" } };
            _mockService.Setup(s => s.GetAllSuppliersAsync()).ReturnsAsync(suppliers);

            // Act
            var result = await _controller.GetSuppliers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<ApiResponse<IEnumerable<SupplierDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task GetSuppliers_Negative_ThrowsException()
        {
            // Arrange
            _mockService.Setup(s => s.GetAllSuppliersAsync()).ThrowsAsync(new DataNotFoundException("No Suppliers found"));

            // Act & Assert
            await Assert.ThrowsAsync<DataNotFoundException>(() => _controller.GetSuppliers());
        }

        // 2. GetSupplier
        [Fact]
        public async Task GetSupplier_Positive_ReturnsOk()
        {
            // Arrange
            var supplier = new SupplierDto { SupplierId = 1, Name = "Test" };
            _mockService.Setup(s => s.GetSupplierByIdAsync(1)).ReturnsAsync(supplier);

            // Act
            var result = await _controller.GetSupplier(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<ApiResponse<SupplierDto>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(1, response.Data.SupplierId);
        }

        [Fact]
        public async Task GetSupplier_Negative_ThrowsException()
        {
            // Arrange
            _mockService.Setup(s => s.GetSupplierByIdAsync(1)).ThrowsAsync(new SupplierNotFoundException("Supplier not found"));

            // Act & Assert
            await Assert.ThrowsAsync<SupplierNotFoundException>(() => _controller.GetSupplier(1));
        }

        // 3. PostSupplier
        [Fact]
        public async Task PostSupplier_Positive_ReturnsCreated()
        {
            // Arrange
            var createDto = new CreateSupplierDto { Name = "Test" };
            var createdDto = new SupplierDto { SupplierId = 1, Name = "Test" };
            _mockService.Setup(s => s.CreateSupplierAsync(createDto)).ReturnsAsync(createdDto);

            // Act
            var result = await _controller.PostSupplier(createDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var response = Assert.IsType<ApiResponse<SupplierDto>>(createdResult.Value);
            Assert.True(response.Success);
            Assert.Equal(1, response.Data.SupplierId);
        }

        [Fact]
        public async Task PostSupplier_Negative_ThrowsException()
        {
            // Arrange
            var createDto = new CreateSupplierDto { Name = "Test" };
            _mockService.Setup(s => s.CreateSupplierAsync(createDto)).ThrowsAsync(new Exception("Creation failed"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _controller.PostSupplier(createDto));
        }

        // 4. PatchSupplier
        [Fact]
        public async Task PatchSupplier_Positive_ReturnsOk()
        {
            // Arrange
            var patchDoc = new JsonPatchDocument<UpdateSupplierDto>();
            _mockService.Setup(s => s.PatchSupplierAsync(1, patchDoc)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.PatchSupplier(1, patchDoc);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<ApiResponse<string>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal("Supplier updated successfully.", response.Data);
        }

        [Fact]
        public async Task PatchSupplier_Negative_ReturnsBadRequest_WhenDocIsNull()
        {
            // Act
            var result = await _controller.PatchSupplier(1, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var response = Assert.IsType<ApiResponse<string>>(badRequestResult.Value);
            Assert.False(response.Success);
            Assert.Equal("Invalid patch document.", response.Message);
        }

        // 5. GetAllPets
        [Fact]
        public async Task GetAllPets_Positive_ReturnsOk()
        {
            // Arrange
            var pets = new List<PetDto> { new PetDto { PetId = 1, Name = "Buddy" } };
            _mockService.Setup(s => s.GetPetsAsync(1)).ReturnsAsync(pets);

            // Act
            var result = await _controller.GetAllPets(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<ApiResponse<IEnumerable<PetDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Single(response.Data);
        }

        [Fact]
        public async Task GetAllPets_Negative_ThrowsException()
        {
            // Arrange
            _mockService.Setup(s => s.GetPetsAsync(1)).ThrowsAsync(new DataNotFoundException("No Pets are Linked"));

            // Act & Assert
            await Assert.ThrowsAsync<DataNotFoundException>(() => _controller.GetAllPets(1));
        }
    }
}
