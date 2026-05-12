using System;
using System.Collections.Generic;

namespace PetManagementSystem.Api.Models;

public partial class Supplier
{
   public int SupplierId { get; set; }

    public string? Name { get; set; }

    public string? ContactPerson { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public int? AddressId { get; set; }

    public string PasswordHash { get; set; } = null!;

    public virtual Address? Address { get; set; }

    public virtual ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
