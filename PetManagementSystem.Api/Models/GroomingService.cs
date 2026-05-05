using System;
using System.Collections.Generic;

namespace PetManagementSystem.Api.Models;

public partial class GroomingService
{
    public int ServiceId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public byte? Available { get; set; }

    public virtual ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
