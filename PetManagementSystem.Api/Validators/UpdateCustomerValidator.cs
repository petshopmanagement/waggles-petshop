using FluentValidation;
using PetManagementSystem.Api.DTOs;

namespace PetManagementSystem.Api.Validators;

public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerDto>
{
    public UpdateCustomerValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First Name is required.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last Name is required.");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone Number is required.");
    }
}
