using System;
using System.Collections.Generic;

namespace PetManagementSystem.Api.Models;

public partial class Address
{
    public int AddressId { get; set; }

    public string? Street { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? ZipCode { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();
}
