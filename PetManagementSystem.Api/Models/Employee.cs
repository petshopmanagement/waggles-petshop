using System;
using System.Collections.Generic;

namespace PetManagementSystem.Api.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Position { get; set; }

    public DateOnly? HireDate { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public int? AddressId { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
