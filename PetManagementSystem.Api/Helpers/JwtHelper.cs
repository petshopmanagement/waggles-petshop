using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetManagementSystem.Api.DTOs;

namespace PetManagementSystem.Api.Helpers;

public class JwtHelper
{
    private readonly IConfiguration _configuration;

    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AuthResponse GenerateToken(string id, string email, string role, string? name)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var secretKey = jwtSettings["Key"] ?? "super_secret_key_that_is_long_enough_for_hmac_sha256_and_even_longer_now_for_hs256_algo";

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, id),
            new Claim(ClaimTypes.Email, email ?? string.Empty),
            new Claim(ClaimTypes.Role, role ?? string.Empty),
            new Claim(ClaimTypes.Name, name ?? string.Empty)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"] ?? "PetStoreApi",
            audience: jwtSettings["Audience"] ?? "PetStoreClients",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new AuthResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Role = role ?? string.Empty,
            Email = email ?? string.Empty,
            Name = name ?? string.Empty,
            UserId = id
        };
    }
}
