using PetManagementSystem.Api.DTOs;

namespace PetManagementSystem.Api.Services
{
    public interface IVaccinationService
    {
        Task<IEnumerable<VaccinationDto>> GetAllAsync();
        Task<VaccinationDto?> GetByIdAsync(int id);
        Task<VaccinationDto> CreateAsync(CreateVaccinationDto dto);
        Task<VaccinationDto> UpdateAsync(int id, UpdateVaccinationDto dto);
        Task<VaccinationDto?> PatchAsync(int id, UpdateVaccinationDto dto);
        Task<IEnumerable<PetDto>> GetPetsAsync(int id);
    }
}
