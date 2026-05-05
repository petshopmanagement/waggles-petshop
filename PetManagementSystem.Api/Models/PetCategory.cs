using System;
using System.Collections.Generic;

namespace PetManagementSystem.Api.Models;

public partial class PetCategory
{
    public int CategoryId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
