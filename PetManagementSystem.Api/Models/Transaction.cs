using System;
using System.Collections.Generic;

namespace PetManagementSystem.Api.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int? CustomerId { get; set; }

    public int? PetId { get; set; }

    public DateOnly? TransactionDate { get; set; }

    public decimal? Amount { get; set; }

    public string? TransactionStatus { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Pet? Pet { get; set; }
}
