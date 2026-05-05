using System;
using System.Collections.Generic;

namespace PetManagementSystem.Api.Models;

public partial class PetFood
{
    public int FoodId { get; set; }

    public string? Name { get; set; }

    public string? Brand { get; set; }

    public string? Type { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
