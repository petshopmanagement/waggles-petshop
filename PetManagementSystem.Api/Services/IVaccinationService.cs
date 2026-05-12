using PetManagementSystem.Api.DTOs;

namespace PetManagementSystem.Api.Services
{
    public interface IVaccinationService
    {
        Task<IEnumerable<VaccinationDto>> GetAllAsync();
        Task<VaccinationDto?> GetByIdAsync(int id);
        Task<VaccinationDto> CreateAsync(WriteVaccinationDto dto);
        Task<VaccinationDto> UpdateAsync(int id, WriteVaccinationDto dto);
        Task<VaccinationDto?> PatchAsync(int id, WriteVaccinationDto dto);
        Task<IEnumerable<PetDto>> GetPetsAsync(int id);
    }
}
