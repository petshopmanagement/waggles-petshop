using AutoMapper;
using Moq;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Exceptions;
using PetManagementSystem.Api.Models;
using PetManagementSystem.Api.Repositories;
using PetManagementSystem.Api.Services;

namespace PetManagementSystem.Tests;

public class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CustomerService _sut;

    public CustomerServiceTests()
    {
        _repoMock = new Mock<ICustomerRepository>();
        _mapperMock = new Mock<IMapper>();
        _sut = new CustomerService(_repoMock.Object, _mapperMock.Object);
    }

  

    [Fact]
    public async Task Positive1_GetAllAsync_ReturnsAllCustomersAsDtos()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new() { CustomerId = 1, FirstName = "Alice" },
            new() { CustomerId = 2, FirstName = "Bob" }
        };
        var expectedDtos = new List<CustomerDto>
        {
            new() { CustomerId = 1, FirstName = "Alice" },
            new() { CustomerId = 2, FirstName = "Bob" }
        };

        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(customers);
        _mapperMock.Setup(m => m.Map<IEnumerable<CustomerDto>>(customers)).Returns(expectedDtos);

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, r => r.FirstName == "Alice");
        Assert.Contains(result, r => r.FirstName == "Bob");
    }

    [Fact]
    public async Task Positive2_GetByIdAsync_WhenCustomerExists_ReturnsMappedDto()
    {
        // Arrange
        var customer = new Customer { CustomerId = 5, FirstName = "Charlie", Email = "charlie@email.com" };
        var dto = new CustomerDto { CustomerId = 5, FirstName = "Charlie", Email = "charlie@email.com" };

        _repoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(customer);
        _mapperMock.Setup(m => m.Map<CustomerDto>(customer)).Returns(dto);

        // Act
        var result = await _sut.GetByIdAsync(5);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result!.CustomerId);
        Assert.Equal("Charlie", result.FirstName);
    }

    [Fact]
    public async Task Positive3_GetProfileAsync_WhenCustomerExists_ReturnsProfileWithAddress()
    {
        // Arrange
        var customer = new Customer
        {
            CustomerId = 3,
            FirstName = "Diana",
            Address = new Address { Street = "123 Main St", City = "Springfield" }
        };
        var profileDto = new CustomerProfileDto { CustomerId = 3, FirstName = "Diana" };

        _repoMock.Setup(r => r.GetWithAddressAsync(3)).ReturnsAsync(customer);
        _mapperMock.Setup(m => m.Map<CustomerProfileDto>(customer)).Returns(profileDto);

        // Act
        var result = await _sut.GetProfileAsync(3);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result!.CustomerId);
        Assert.Equal("Diana", result.FirstName);
    }

    [Fact]
    public async Task Positive4_PatchAsync_WhenPartialUpdate_UpdatesOnlyProvidedFields()
    {
        // Arrange — only Email is being patched, FirstName must stay unchanged
        var existing = new Customer { CustomerId = 7, FirstName = "Eve", Email = "old@mail.com" };
        var patchDto = new PatchCustomerDto { Email = "new@mail.com" };
        var updatedDto = new CustomerDto { CustomerId = 7, FirstName = "Eve", Email = "new@mail.com" };

        _repoMock.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(existing);
        _repoMock.Setup(r => r.UpdateAsync(7, existing)).ReturnsAsync(existing);
        _mapperMock.Setup(m => m.Map<CustomerDto>(existing)).Returns(updatedDto);

        // Act
        var result = await _sut.PatchAsync(7, patchDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("new@mail.com", result!.Email);
        Assert.Equal("Eve", existing.FirstName); // FirstName was NOT in patch → must be unchanged
    }

    [Fact]
    public async Task Positive5_DeleteAsync_WhenCustomerExists_ReturnsTrue()
    {
        // Arrange
        _repoMock.Setup(r => r.DeleteAsync(10)).ReturnsAsync(true);

        // Act
        var result = await _sut.DeleteAsync(10);

        // Assert
        Assert.True(result);
        _repoMock.Verify(r => r.DeleteAsync(10), Times.Once); // repo was actually called
    }


    [Fact]
    public async Task Negative1_GetByIdAsync_WhenCustomerDoesNotExist_ThrowsCustomerNotFoundException()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Customer?)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<CustomerNotFoundException>(
            () => _sut.GetByIdAsync(999));

        Assert.Contains("999", ex.Message); // message should mention the bad ID
    }

    [Fact]
    public async Task Negative2_GetProfileAsync_WhenCustomerDoesNotExist_ThrowsCustomerNotFoundException()
    {
        // Arrange
        _repoMock.Setup(r => r.GetWithAddressAsync(888)).ReturnsAsync((Customer?)null);

        // Act & Assert
        await Assert.ThrowsAsync<CustomerNotFoundException>(
            () => _sut.GetProfileAsync(888));
    }

    [Fact]
    public async Task Negative3_PatchAsync_WhenCustomerDoesNotExist_ThrowsCustomerNotFoundException()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(777)).ReturnsAsync((Customer?)null);

        // Act & Assert
        await Assert.ThrowsAsync<CustomerNotFoundException>(
            () => _sut.PatchAsync(777, new PatchCustomerDto { FirstName = "Ghost" }));
    }

    [Fact]
    public async Task Negative4_UpdateAsync_WhenRepoReturnsNull_ReturnsNullToController()
    {
        // Arrange — simulates the case where the record was deleted between GET and UPDATE
        var updateDto = new UpdateCustomerDto { FirstName = "Nobody" };
        var entity = new Customer { FirstName = "Nobody" };

        _mapperMock.Setup(m => m.Map<Customer>(updateDto)).Returns(entity);
        _repoMock.Setup(r => r.UpdateAsync(666, entity)).ReturnsAsync((Customer?)null);

        // Act
        var result = await _sut.UpdateAsync(666, updateDto);

        // Assert
        Assert.Null(result); // service must propagate null — controller handles the 404
    }

    [Fact]
    public async Task Negative5_DeleteAsync_WhenCustomerDoesNotExist_ReturnsFalse()
    {
        // Arrange
        _repoMock.Setup(r => r.DeleteAsync(555)).ReturnsAsync(false);

        // Act
        var result = await _sut.DeleteAsync(555);

        // Assert
        Assert.False(result);
        _repoMock.Verify(r => r.DeleteAsync(555), Times.Once);
    }
}
