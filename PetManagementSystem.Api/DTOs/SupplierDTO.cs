namespace PetManagementSystem.Api.DTOs
{
    public class SupplierDTO
    {
        public int SupplierId { get; set; }
        public string? Name { get; set; }
        public string? ContactPerson { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public AddressDto? Address { get; set; }
        public List<PetDto>? Pets { get; set; }
    }
}