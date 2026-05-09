using Microsoft.AspNetCore.JsonPatch;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.DTOs.SupplierDtos;
using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Services
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync();
        Task<SupplierDto?> GetSupplierByIdAsync(int id);
        Task<SupplierDto> CreateSupplierAsync(CreateSupplierDto dto);
        Task PatchSupplierAsync(int id, JsonPatchDocument<UpdateSupplierDto> patchDoc);
        Task<IEnumerable<PetDto>> GetPetsAsync(int id);
    }
}
