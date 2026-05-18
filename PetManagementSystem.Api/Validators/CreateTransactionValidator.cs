using FluentValidation;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Helpers;

namespace PetManagementSystem.Api.Validators;

public class CreateTransactionValidator : AbstractValidator<CreateTransactionDto>
{
    public CreateTransactionValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotNull().WithMessage("CustomerId is required.")
            .GreaterThan(0).WithMessage("CustomerId must be a positive integer.");

        RuleFor(x => x.PetId)
            .NotNull().WithMessage("PetId is required.")
            .GreaterThan(0).WithMessage("PetId must be a positive integer.");

        RuleFor(x => x.Amount)
            .NotNull().WithMessage("Amount is required.")
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.TransactionDate)
            .NotNull().WithMessage("Transaction date is required.")
            .Must(d => d <= DateOnly.FromDateTime(DateTime.Now).AddDays(1))
            .WithMessage("Transaction date cannot be in the future.");

        RuleFor(x => x.TransactionStatus)
            .NotEmpty().WithMessage("Transaction status is required.")
            .Must(s => TransactionHelper.IsValidStatus(s))
            .WithMessage($"Status must be one of: {string.Join(", ", TransactionHelper.ValidStatuses)}.");
    }
}
