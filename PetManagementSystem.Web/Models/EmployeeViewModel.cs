namespace PetManagementSystem.Web.Models
{
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Position { get; set; }
        public DateOnly? HireDate { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public AddressViewModel? Address { get; set; }
        public List<PetViewModel>? Pets { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }

    public class AddressViewModel
    {
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
    }
}
