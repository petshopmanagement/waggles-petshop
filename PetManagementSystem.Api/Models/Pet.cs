using System;
using System.Collections.Generic;

namespace PetManagementSystem.Api.Models;

public partial class Pet
{
    public int PetId { get; set; }

    public string? Name { get; set; }

    public string? Breed { get; set; }

    public int? Age { get; set; }

    public decimal? Price { get; set; }

    public int? CategoryId { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public virtual PetCategory? Category { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<PetFood> Foods { get; set; } = new List<PetFood>();

    public virtual ICollection<GroomingService> Services { get; set; } = new List<GroomingService>();

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();

    public virtual ICollection<Vaccination> Vaccinations { get; set; } = new List<Vaccination>();
}
