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
    public class CreateEmployeeDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Position { get; set; }
        public DateOnly? HireDate { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public int? AddressId { get; set; }
    }
    public class UpdateEmployeeDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Position { get; set; }
        public DateOnly? HireDate { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public int? AddressId { get; set; }
    }
}