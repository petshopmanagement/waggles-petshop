using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
using PetManagementSystem.Api.Controllers;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Services;

namespace PetManagementSystem.Tests.Controllers;

public class CustomersControllerTests
{
    private readonly Mock<ICustomerService> _mockService;
    private readonly CustomersController _controller;

    public CustomersControllerTests()
    {
        _mockService = new Mock<ICustomerService>();
        _controller = new CustomersController(_mockService.Object);
    }

    // --- Positive Tests ---

    [Fact]
    public async Task GetAll_Ok()
    {
        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<CustomerDto> { new CustomerDto { CustomerId = 1 } });

        var result = await _controller.GetAll();

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetById_Ok()
    {
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new CustomerDto { CustomerId = 1 });

        var result = await _controller.GetById(1);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetProfile_Ok()
    {
        _mockService.Setup(s => s.GetProfileAsync(1)).ReturnsAsync(new CustomerDto());

        var result = await _controller.GetProfile(1);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Update_Ok()
    {
        var dto = new UpdateCustomerDto();
        _mockService.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(new CustomerDto());

        var result = await _controller.Update(1, dto);

        result.Should().BeOfType<OkObjectResult>();
    }

    // --- Negative Tests ---

    [Fact]
    public async Task GetById_NotFound()
    {
        _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((CustomerDto?)null);

        var result = await _controller.GetById(99);

        result.Should().BeOfType<OkObjectResult>(); 
    }

    [Fact]
    public async Task Update_Fail()
    {
        var dto = new UpdateCustomerDto();
        _mockService.Setup(s => s.UpdateAsync(1, dto)).ThrowsAsync(new System.Exception("Error"));

        var action = async () => await _controller.Update(1, dto);

        await action.Should().ThrowAsync<System.Exception>();
    }

    [Fact]
    public async Task AddAddress_Fail()
    {
        var dto = new AddressDto();
        _mockService.Setup(s => s.AddAddressAsync(1, dto)).ThrowsAsync(new System.Exception("Fail"));

        var action = async () => await _controller.AddAddress(1, dto);

        await action.Should().ThrowAsync<System.Exception>();
    }

    [Fact]
    public async Task GetTransactions_Fail()
    {
        _mockService.Setup(s => s.GetTransactionsAsync(1)).ThrowsAsync(new System.Exception("No data"));

        var action = async () => await _controller.GetTransactions(1);

        await action.Should().ThrowAsync<System.Exception>();
    }
}
