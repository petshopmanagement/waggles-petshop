using FluentValidation;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Helpers;

namespace PetManagementSystem.Api.Validators;

public class UpdateTransactionStatusValidator : AbstractValidator<UpdateTransactionStatusDto>
{
    public UpdateTransactionStatusValidator()
    {
        RuleFor(x => x.TransactionStatus)
            .NotEmpty().WithMessage("Transaction status is required.")
            .Must(s => TransactionHelper.IsValidStatus(s))
            .WithMessage($"Status must be one of: {string.Join(", ", TransactionHelper.ValidStatuses)}.");
    }
}
