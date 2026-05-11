using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
using PetManagementSystem.Api.Controllers;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Exceptions;
using PetManagementSystem.Api.Services;

namespace PetManagementSystem.Tests.Controllers;

public class VaccinationsControllerTests
{
    private readonly Mock<IVaccinationService> _mockService;
    private readonly VaccinationsController _controller;

    public VaccinationsControllerTests()
    {
        _mockService = new Mock<IVaccinationService>();
        _controller = new VaccinationsController(_mockService.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturn200_WhenVaccinationsExist()
    {
        // Arrange
        var vaccinationList = new List<VaccinationDto>
        {
            new VaccinationDto { VaccinationId = 1, Name = "Rabies", Description = "Rabies vaccine", Price = 500, Available = true },
            new VaccinationDto { VaccinationId = 2, Name = "Distemper", Description = "Distemper vaccine", Price = 350, Available = true }
        };

        _mockService
            .Setup(s => s.GetAllAsync())
            .ReturnsAsync(vaccinationList);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }


    [Fact]
    public async Task GetAll_ShouldThrowException_WhenServiceFails()
    {
        // Arrange
        _mockService
            .Setup(s => s.GetAllAsync())
            .ThrowsAsync(new Exception("Database connection failed"));

        // Act
        var action = async () => await _controller.GetAll();

        // Assert
        await action.Should()
            .ThrowAsync<Exception>()
            .WithMessage("Database connection failed");
    }


    [Fact]
    public async Task GetById_ShouldReturn200_WhenVaccinationExists()
    {
        // Arrange
        var vaccinationId = 1;

        var vaccination = new VaccinationDto
        {
            VaccinationId = vaccinationId,
            Name = "Rabies",
            Description = "Rabies vaccine for dogs",
            Price = 500,
            Available = true
        };

        _mockService
            .Setup(s => s.GetByIdAsync(vaccinationId))
            .ReturnsAsync(vaccination);

        // Act
        var result = await _controller.GetById(vaccinationId);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }


    [Fact]
    public async Task GetById_ShouldThrowVaccinationNotFoundException_WhenVaccinationDoesNotExist()
    {
        // Arrange
        var vaccinationId = 99;

        _mockService
            .Setup(s => s.GetByIdAsync(vaccinationId))
            .ThrowsAsync(new VaccinationNotFoundException($"Vaccination with Id {vaccinationId} not found"));

        // Act
        var action = async () => await _controller.GetById(vaccinationId);

        // Assert
        await action.Should()
            .ThrowAsync<VaccinationNotFoundException>()
            .WithMessage($"Vaccination with Id {vaccinationId} not found");
    }


    [Fact]
    public async Task Create_ShouldReturn201_WhenVaccinationCreatedSuccessfully()
    {
        // Arrange
        var createDto = new CreateVaccinationDto
        {
            Name = "Parvo",
            Description = "Parvovirus vaccine",
            Price = 450,
            Available = true
        };

        var createdVaccination = new VaccinationDto
        {
            VaccinationId = 3,
            Name = "Parvo",
            Description = "Parvovirus vaccine",
            Price = 450,
            Available = true
        };

        _mockService
            .Setup(s => s.CreateAsync(createDto))
            .ReturnsAsync(createdVaccination);

        // Act
        var result = await _controller.Create(createDto);

        // Assert
        var createdResult = result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdResult.StatusCode.Should().Be(201);
    }


    [Fact]
    public async Task Create_ShouldThrowException_WhenVaccinationDataIsInvalid()
    {
        // Arrange
        var createDto = new CreateVaccinationDto
        {
            Name = null,  
            Description = null,
            Price = -100,  
            Available = true
        };

        _mockService
            .Setup(s => s.CreateAsync(createDto))
            .ThrowsAsync(new Exception("Vaccination name is required"));

        // Act
        var action = async () => await _controller.Create(createDto);

        // Assert
        await action.Should()
            .ThrowAsync<Exception>()
            .WithMessage("Vaccination name is required");
    }


    [Fact]
    public async Task Update_ShouldReturn200_WhenVaccinationUpdatedSuccessfully()
    {
        // Arrange
        var vaccinationId = 1;

        var updateDto = new UpdateVaccinationDto
        {
            Name = "Rabies Updated",
            Description = "Updated rabies vaccine",
            Price = 550,
            Available = true
        };

        var updatedVaccination = new VaccinationDto
        {
            VaccinationId = vaccinationId,
            Name = "Rabies Updated",
            Description = "Updated rabies vaccine",
            Price = 550,
            Available = true
        };

        _mockService
            .Setup(s => s.UpdateAsync(vaccinationId, updateDto))
            .ReturnsAsync(updatedVaccination);

        // Act
        var result = await _controller.Update(vaccinationId, updateDto);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.StatusCode.Should().Be(200);
    }


    [Fact]
    public async Task Update_ShouldThrowVaccinationNotFoundException_WhenVaccinationDoesNotExist()
    {
        // Arrange
        var vaccinationId = 99;

        var updateDto = new UpdateVaccinationDto
        {
            Name = "Non-existent Vaccine",
            Price = 200
        };

        _mockService
            .Setup(s => s.UpdateAsync(vaccinationId, updateDto))
            .ThrowsAsync(new VaccinationNotFoundException($"Vaccination with Id {vaccinationId} not found"));

        // Act
        var action = async () => await _controller.Update(vaccinationId, updateDto);

        // Assert
        await action.Should()
            .ThrowAsync<VaccinationNotFoundException>()
            .WithMessage($"Vaccination with Id {vaccinationId} not found");
    }
}
