namespace PetManagementSystem.Web.Models
{
    public class AddressViewModel
    {
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
    }

    public class SupplierViewModel
    {
        public int SupplierId { get; set; }
        public string? Name { get; set; }
        public string? ContactPerson { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public AddressViewModel? Address { get; set; }
    }
}
