namespace PetManagementSystem.Api.DTOs
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Position { get; set; }
        public DateOnly? HireDate { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public AddressDto? Address { get; set; }

    }
    public class WriteEmployeeDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Position { get; set; }
        public DateOnly? HireDate { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public WriteAddressDto? Address { get; set; }
    }

}