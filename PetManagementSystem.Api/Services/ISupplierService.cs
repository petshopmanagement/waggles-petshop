using Microsoft.AspNetCore.JsonPatch;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Models;

namespace PetManagementSystem.Api.Services
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDto>> GetAllSuppliersAsync();
        Task<SupplierDto?> GetSupplierByIdAsync(int id);
        Task<SupplierDto> CreateSupplierAsync(SupplierDto dto);
        Task PatchSupplierAsync(int id, JsonPatchDocument<SupplierDto> patchDoc);
        Task<IEnumerable<PetDto>> GetPetsAsync(int id);
        Task<AddressDto?> GetAddressAsync(int id);
    }
}
