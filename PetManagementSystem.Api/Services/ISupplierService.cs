using Microsoft.AspNetCore.JsonPatch;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Services
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDTO>> GetAllSuppliersAsync();
        Task<SupplierDTO?> GetSupplierByIdAsync(int id);
        Task<SupplierDTO> CreateSupplierAsync(SupplierDTO dto);
        Task PatchSupplierAsync(int id, JsonPatchDocument<SupplierDTO> patchDoc);
        Task<IEnumerable<PetDto>> GetPetsAsync(int id);
    }
}