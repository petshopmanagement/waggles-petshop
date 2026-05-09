using FluentValidation;
using PetManagementSystem.Api.DTOs;

namespace PetManagementSystem.Api.Validators;

public class RegisterValidator : AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required.")
            .Must(x => new[] { "customer", "employee", "supplier" }.Contains(x?.ToLower()))
            .WithMessage("Role must be 'Customer', 'Employee', or 'Supplier'.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

        // Customer & Employee validation
        When(x => x.Role?.ToLower() == "customer" || x.Role?.ToLower() == "employee", () =>
        {
            RuleFor(x => x.FirstName).
            NotEmpty().
            WithMessage("First Name is required for this role.");
            RuleFor(x => x.LastName).NotEmpty().
            WithMessage("Last Name is required for this role.");
        });

        // Supplier validation
        When(x => x.Role?.ToLower() == "supplier", () =>
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Company Name is required for Supplier.");
        });

        // Address validation using AddressValidator
        RuleFor(x => x.Address).SetValidator(new AddressValidator()!)
            .When(x => x.Address != null);
    }
}
