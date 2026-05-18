using PetManagementSystem.Api.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace PetManagementSystem.Api.Services
{
    public interface IGroomingServiceService
    {
        Task<IEnumerable<GroomingDTO>> GetAllAsync();
        Task<GroomingDTO?> GetByIdAsync(int id);
        Task<GroomingDTO> CreateAsync(GroomingDTO dto);
        Task PatchAsync(int id, JsonPatchDocument<GroomingDTO> patchDoc);
        Task<IEnumerable<PetDto>> GetPetsAsync(int id);
    }
}