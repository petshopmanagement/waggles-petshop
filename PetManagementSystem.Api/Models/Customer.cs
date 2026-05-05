using System;
using System.Collections.Generic;

namespace PetManagementSystem.Api.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public int? AddressId { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
