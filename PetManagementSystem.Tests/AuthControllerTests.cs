using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
using PetManagementSystem.Api.Controllers;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Services;
using PetManagementSystem.Api.Helpers;

namespace PetManagementSystem.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _mockService;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mockService = new Mock<IAuthService>();
        _controller = new AuthController(_mockService.Object);
    }

    // --- Positive Tests ---

    [Fact]
    public async Task Login_Ok()
    {
        var request = new LoginRequest { Email = "test@user.com", Password = "Password123" };
        var response = new AuthResponse { Token = "valid-token", Role = "Customer" };
        _mockService.Setup(s => s.LoginAsync(request)).ReturnsAsync(response);

        var result = await _controller.Login(request);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Register_Ok()
    {
        var request = new RegisterRequest { Email = "new@user.com", Password = "Password123" };
        _mockService.Setup(s => s.RegisterAsync(request)).ReturnsAsync("Success");

        var result = await _controller.Register(request);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task ChangePassword_Ok()
    {
        var request = new ChangePasswordRequest { Email = "test@user.com", OldPassword = "123", NewPassword = "456" };
        
        var result = await _controller.ChangePassword(request);

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Login_ReturnsData()
    {
        var request = new LoginRequest { Email = "test@user.com" };
        _mockService.Setup(s => s.LoginAsync(request)).ReturnsAsync(new AuthResponse { Token = "T123" });

        var result = await _controller.Login(request) as OkObjectResult;
        var data = result?.Value as ApiResponse<AuthResponse>;

        data?.Data?.Token.Should().Be("T123");
    }

    // --- Negative Tests ---

    [Fact]
    public async Task Login_Fail()
    {
        var request = new LoginRequest { Email = "wrong@user.com" };
        _mockService.Setup(s => s.LoginAsync(request)).ThrowsAsync(new System.Exception("Invalid"));

        var action = async () => await _controller.Login(request);

        await action.Should().ThrowAsync<System.Exception>();
    }

    [Fact]
    public async Task Register_Fail()
    {
        var request = new RegisterRequest { Email = "exists@user.com" };
        _mockService.Setup(s => s.RegisterAsync(request)).ThrowsAsync(new System.Exception("Email taken"));

        var action = async () => await _controller.Register(request);

        await action.Should().ThrowAsync<System.Exception>();
    }

    [Fact]
    public async Task ChangePassword_Fail()
    {
        var request = new ChangePasswordRequest();
        _mockService.Setup(s => s.ChangePasswordAsync(request)).ThrowsAsync(new System.Exception("Error"));

        var action = async () => await _controller.ChangePassword(request);

        await action.Should().ThrowAsync<System.Exception>();
    }

    [Fact]
    public async Task Login_InvalidModel()
    {
        _controller.ModelState.AddModelError("Email", "Required");
        var request = new LoginRequest();

        var result = await _controller.Login(request);

        result.Should().BeOfType<BadRequestObjectResult>();
    }
}
