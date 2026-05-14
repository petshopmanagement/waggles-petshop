using System;

namespace PetManagementSystem.Web.Models
{
    public class CreateTransactionDto
    {
        public int? CustomerId { get; set; }
        public int? PetId { get; set; }
        public DateOnly? TransactionDate { get; set; }
        public decimal? Amount { get; set; }
        public string? TransactionStatus { get; set; }
    }
}
