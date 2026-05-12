namespace PetManagementSystem.Api.DTOs;

public class CustomerDto
{
    public int CustomerId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    
    public AddressDto? Address { get; set; }
    public List<TransactionDto>? Transactions { get; set; }
}




public class UpdateCustomerDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

    public int? AddressId { get; set; }
}
