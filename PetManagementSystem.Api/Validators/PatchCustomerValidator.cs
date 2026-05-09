using FluentValidation;
using PetManagementSystem.Api.DTOs;

namespace PetManagementSystem.Api.Validators;

public class PatchCustomerValidator : AbstractValidator<PatchCustomerDto>
{
    public PatchCustomerValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("A valid email address is required.")
            .When(x => !string.IsNullOrEmpty(x.Email));
    }
}
