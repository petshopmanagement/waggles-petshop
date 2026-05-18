using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
using PetManagementSystem.Api.Controllers;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Services;

namespace PetManagementSystem.Tests.Controllers;

public class TransactionsControllerTests
{
    private readonly Mock<ITransactionService> _mockService;
    private readonly TransactionsController _controller;

    public TransactionsControllerTests()
    {
        _mockService = new Mock<ITransactionService>();
        _controller = new TransactionsController(_mockService.Object);
    }

    // --- Positive Tests ---

    [Fact]
    public async Task GetAll_Ok()
    {
        _mockService.Setup(s => s.GetAllAsync(1, 10)).ReturnsAsync(new List<TransactionDto>());

        var result = await _controller.GetAll();

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task GetById_Ok()
    {
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(new TransactionDto { TransactionId = 1 });

        var result = await _controller.GetById(1);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Create_Ok()
    {
        var dto = new CreateTransactionDto { PetId = 1, CustomerId = 1 };
        var created = new TransactionDto { TransactionId = 10 };
        _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

        var result = await _controller.Create(dto);

        result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async Task GetTotalRevenue_Ok()
    {
        _mockService.Setup(s => s.GetTotalRevenueAsync()).ReturnsAsync(5000m);

        var result = await _controller.GetTotalRevenue();

        result.Should().BeOfType<OkObjectResult>();
    }

    // --- Negative Tests ---

    [Fact]
    public async Task GetById_Fail()
    {
        _mockService.Setup(s => s.GetByIdAsync(99)).ThrowsAsync(new System.Exception("Not found"));

        var action = async () => await _controller.GetById(99);

        await action.Should().ThrowAsync<System.Exception>();
    }

    [Fact]
    public async Task Create_Fail()
    {
        var dto = new CreateTransactionDto();
        _mockService.Setup(s => s.CreateAsync(dto)).ThrowsAsync(new System.Exception("Invalid data"));

        var action = async () => await _controller.Create(dto);

        await action.Should().ThrowAsync<System.Exception>();
    }

    [Fact]
    public async Task UpdateStatus_Fail()
    {
        var dto = new UpdateTransactionStatusDto();
        _mockService.Setup(s => s.UpdateStatusAsync(1, dto)).ThrowsAsync(new System.Exception("Error"));

        var action = async () => await _controller.UpdateStatus(1, dto);

        await action.Should().ThrowAsync<System.Exception>();
    }

    [Fact]
    public async Task Search_Fail()
    {
        _mockService.Setup(s => s.SearchAsync("abc", null, 1, 10)).ThrowsAsync(new System.Exception("Search failed"));

        var action = async () => await _controller.Search("abc", null);

        await action.Should().ThrowAsync<System.Exception>();
    }
}
