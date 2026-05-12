using System.Text.Json.Serialization;

namespace PetManagementSystem.Api.DTOs
{
    public class SupplierDto
    {
        public int SupplierId { get; set; }
        public string? Name { get; set; }
        public string? ContactPerson { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        //public int? AddressId { get; set; }
        public AddressDto? Address { get; set; }
        public List<PetDto>? Pets { get; set; }
    }
}
