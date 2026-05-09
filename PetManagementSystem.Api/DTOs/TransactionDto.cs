namespace PetManagementSystem.Api.DTOs
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public int? CustomerId { get; set; }
        
        public int? PetId { get; set; }
        public string? PetName { get; set; }
        public DateOnly? TransactionDate { get; set; }
        public decimal? Amount { get; set; }
        public string? TransactionStatus { get; set; }
    }
}
