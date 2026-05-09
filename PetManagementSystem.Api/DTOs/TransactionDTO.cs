namespace PetManagementSystem.Api.DTOs;

public class TransactionDto
{
    public int TransactionId { get; set; }
    public int? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public int? PetId { get; set; }
    public string? PetName { get; set; }
    public DateOnly? TransactionDate { get; set; }
    public decimal? Amount { get; set; }
    public string? TransactionStatus { get; set; }
}

public class CreateTransactionDto
{
    public int? CustomerId { get; set; }
    public int? PetId { get; set; }
    public DateOnly? TransactionDate { get; set; }
    public decimal? Amount { get; set; }
    public string? TransactionStatus { get; set; }
}

public class UpdateTransactionStatusDto
{
    public string? TransactionStatus { get; set; }
}

public class SalesSummaryDto
{
    public int TotalTransactions { get; set; }
    public int SuccessfulTransactions { get; set; }
    public int FailedTransactions { get; set; }
    public decimal TotalRevenue { get; set; }
}
