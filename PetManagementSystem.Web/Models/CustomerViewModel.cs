using System;
using System.Collections.Generic;

namespace PetManagementSystem.Web.Models
{
  
    public class TransactionViewModel
    {
        public int TransactionId { get; set; }
        public string? PetName { get; set; }
        public DateOnly? TransactionDate { get; set; }
        public decimal? Amount { get; set; }
        public string? TransactionStatus { get; set; }
    }

    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public AddressViewModel? Address { get; set; }
    }
}
