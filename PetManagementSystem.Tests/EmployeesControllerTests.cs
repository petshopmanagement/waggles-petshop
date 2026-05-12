using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
using PetManagementSystem.Api.Controllers;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Exceptions;
using PetManagementSystem.Api.Services;

namespace PetManagementSystem.Tests.Controllers;

public class EmployeesControllerTests
{
    private readonly Mock<IEmployeeService> _mockService;
    private readonly EmployeesController _controller;

    public EmployeesControllerTests()
    {
        _mockService = new Mock<IEmployeeService>();
        _controller = new EmployeesController(_mockService.Object);
    }



    [Fact]
    public async Task GetAllEmployees_ShouldReturn200_WhenEmployeesExist()
    {
        // Arrange
        var employeeList = new List<EmployeeDto>
        {
            new EmployeeDto { EmployeeId = 1, FirstName = "Riya", LastName = "Sharma", Position = "Vet", Email = "riya@waggles.com", PhoneNumber = "9876543210" },
            new EmployeeDto { EmployeeId = 2, FirstName = "Arjun", LastName = "Mehta", Position = "Groomer", Email = "arjun@waggles.com", PhoneNumber = "9876543211" }
        };

        _mockService
            .Setup(s => s.GetAllEmployeesAsync())
            .ReturnsAsync(employeeList);

        // Act
        var result = await _controller.GetAllEmployees();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }


    [Fact]
    public async Task GetAllEmployees_ShouldThrowException_WhenServiceFails()
    {
        // Arrange
        _mockService
            .Setup(s => s.GetAllEmployeesAsync())
            .ThrowsAsync(new Exception("Database connection failed"));

        // Act
        var action = async () => await _controller.GetAllEmployees();

        // Assert
        await action.Should()
            .ThrowAsync<Exception>()
            .WithMessage("Database connection failed");
    }


    [Fact]
    public async Task GetByEmpId_ShouldReturn200_WhenEmployeeExists()
    {
        // Arrange
        var employeeId = 1;

        var employee = new EmployeeDto
        {
            EmployeeId = employeeId,
            FirstName = "Riya",
            LastName = "Sharma",
            Position = "Vet",
            Email = "riya@waggles.com",
            PhoneNumber = "9876543210",
            HireDate = new DateOnly(2022, 3, 15)
        };

        _mockService
            .Setup(s => s.GetEmpByIdAsync(employeeId))
            .ReturnsAsync(employee);

        // Act
        var result = await _controller.GetByEmpId(employeeId);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }


    [Fact]
    public async Task GetByEmpId_ShouldReturn404_WhenEmployeeDoesNotExist()
    {
        // Arrange
        var employeeId = 99;

        _mockService
            .Setup(s => s.GetEmpByIdAsync(employeeId))
            .ReturnsAsync((EmployeeDto)null!);

        // Act
        var result = await _controller.GetByEmpId(employeeId);

        // Assert
        var notFoundResult = result.Should().BeOfType<NotFoundResult>().Subject;
        notFoundResult.StatusCode.Should().Be(404);
    }


    [Fact]
    public async Task CreateEmployee_ShouldReturn201_WhenEmployeeCreatedSuccessfully()
    {
        // Arrange
        var createDto = new WriteEmployeeDto
        {
            FirstName = "Priya",
            LastName = "Kapoor",
            Position = "Receptionist",
            Email = "priya@waggles.com",
            PhoneNumber = "9876543212",
            HireDate = new DateOnly(2024, 1, 10)
            //AddressId = 1
        };

        var createdEmployee = new EmployeeDto
        {
            EmployeeId = 3,
            FirstName = "Priya",
            LastName = "Kapoor",
            Position = "Receptionist",
            Email = "priya@waggles.com",
            PhoneNumber = "9876543212",
            HireDate = new DateOnly(2024, 1, 10)
        };

        _mockService
            .Setup(s => s.CreateEmployeeAsync(createDto))
            .ReturnsAsync(createdEmployee);

        // Act
        var result = await _controller.CreateEmployee(createDto);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
    }


    [Fact]
    public async Task CreateEmployee_ShouldThrowEmployeeValidationException_WhenEmailAlreadyExists()
    {
        // Arrange
        var createDto = new WriteEmployeeDto
        {
            FirstName = "Riya",
            LastName = "Sharma",
            Position = "Vet",
            Email = "riya@waggles.com",
            PhoneNumber = "9876543210",
            HireDate = new DateOnly(2022, 3, 15)
            //AddressId = 1
        };

        _mockService
            .Setup(s => s.CreateEmployeeAsync(createDto))
            .ThrowsAsync(new EmployeeValidationException("An employee with this email already exists"));

        // Act
        var action = async () => await _controller.CreateEmployee(createDto);

        // Assert
        await action.Should()
            .ThrowAsync<EmployeeValidationException>()
            .WithMessage("An employee with this email already exists");
    }


    [Fact]
    public async Task UpdateEmployee_ShouldReturn200_WhenEmployeeUpdatedSuccessfully()
    {
        // Arrange
        var employeeId = 1;

        var updateDto = new WriteEmployeeDto
        {
            FirstName = "Riya",
            LastName = "Sharma",
            Position = "Senior Vet",
            Email = "riya.updated@waggles.com",
            PhoneNumber = "9876543299"
        };

        var updatedEmployee = new EmployeeDto
        {
            EmployeeId = employeeId,
            FirstName = "Riya",
            LastName = "Sharma",
            Position = "Senior Vet",
            Email = "riya.updated@waggles.com",
            PhoneNumber = "9876543299"
        };

        _mockService
            .Setup(s => s.UpdateEmployeeAsync(employeeId, updateDto))
            .ReturnsAsync(updatedEmployee);

        // Act
        var result = await _controller.UpdateEmployee(employeeId, updateDto);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }


    [Fact]
    public async Task UpdateEmployee_ShouldThrowEmployeeNotFoundException_WhenEmployeeDoesNotExist()
    {
        // Arrange
        var employeeId = 99;

        var updateDto = new WriteEmployeeDto
        {
            FirstName = "Ghost",
            LastName = "Employee",
            Position = "Unknown"
        };

        _mockService
            .Setup(s => s.UpdateEmployeeAsync(employeeId, updateDto))
            .ThrowsAsync(new EmployeeNotFoundException($"Employee with Id {employeeId} not found"));

        // Act
        var action = async () => await _controller.UpdateEmployee(employeeId, updateDto);

        // Assert
        await action.Should()
            .ThrowAsync<EmployeeNotFoundException>()
            .WithMessage($"Employee with Id {employeeId} not found");
    }
}
