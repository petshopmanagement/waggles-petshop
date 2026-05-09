using System.Threading.Tasks;
using PetManagementSystem.Api.DTOs;

namespace PetManagementSystem.Api.Services;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<string> RegisterAsync(RegisterRequest request);
}
