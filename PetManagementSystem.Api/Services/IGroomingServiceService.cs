using PetManagementSystem.Api.DTOs.GroomingServiceDtos;
using PetManagementSystem.Api.DTOs;
using Microsoft.AspNetCore.JsonPatch;

namespace PetManagementSystem.Api.Services
{
    public interface IGroomingServiceService
    {
        Task<IEnumerable<GroomingServiceDto>> GetAllAsync();
        Task<GroomingServiceDto?> GetByIdAsync(int id);
        Task<GroomingServiceDto> CreateAsync(CreateGroomingServiceDto dto);
        Task PatchAsync(int id, JsonPatchDocument<UpdateGroomingServiceDto> patchDoc);
        Task<IEnumerable<PetDto>> GetPetsAsync(int id);
    }
}
