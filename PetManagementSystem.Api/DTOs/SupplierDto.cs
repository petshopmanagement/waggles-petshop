namespace PetManagementSystem.Api.DTOs
{
    public class SupplierDto
    {
        public int SupplierId { get; set; }
        public string? Name { get; set; }
        public string? ContactPerson { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public int? AddressId { get; set; }
    }
}
