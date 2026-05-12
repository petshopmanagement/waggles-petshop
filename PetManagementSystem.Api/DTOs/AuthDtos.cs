using System.ComponentModel.DataAnnotations;

namespace PetManagementSystem.Api.DTOs;

public class RegisterRequest
{
    [Required]
    public string Role { get; set; } = string.Empty; 

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    
    public string? PhoneNumber { get; set; }

   
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public string? Position { get; set; } 

   
    public string? Name { get; set; } 
    public string? ContactPerson { get; set; }
    public CreateAddressDto? Address { get; set; }
}

public class LoginRequest
{
    [Required]
    public string Role { get; set; } = string.Empty; 

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
public class ChangePasswordRequest
{
    [Required]
    public string Role { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string OldPassword { get; set; } = string.Empty;

    [Required]
    public string NewPassword { get; set; } = string.Empty;
}
