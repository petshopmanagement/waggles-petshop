using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Services;
using PetManagementSystem.Api.Helpers; // Added
using System.Threading.Tasks;

namespace PetManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _authService.LoginAsync(request);
            return Ok(ApiResponse<AuthResponse>.SuccessResponse(response));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ApiResponse<AuthResponse>.FailureResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<AuthResponse>.FailureResponse(ex.Message));
        }
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var message = await _authService.RegisterAsync(request);
            return Ok(ApiResponse<string>.SuccessResponse(message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<string>.FailureResponse(ex.Message));
        }
    }


    [HttpPost("change-password")]
    
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _authService.ChangePasswordAsync(request);
            return Ok(ApiResponse<string>.SuccessResponse("Password changed successfully."));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<string>.FailureResponse(ex.Message));
        }
    }
}