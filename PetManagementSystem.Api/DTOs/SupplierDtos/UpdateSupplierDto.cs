namespace PetManagementSystem.Api.DTOs.SupplierDtos
{
    public class UpdateSupplierDto
    {
        public string? Name { get; set; }
        public string? ContactPerson { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public int? AddressId { get; set; }
    }
}
