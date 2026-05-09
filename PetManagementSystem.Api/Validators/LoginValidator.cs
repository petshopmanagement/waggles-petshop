using FluentValidation;
using PetManagementSystem.Api.DTOs;

namespace PetManagementSystem.Api.Validators;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required.")
            .Must(x => new[] { "customer", "employee", "supplier" }.Contains(x.ToLower()))
            .WithMessage("Role must be 'Customer', 'Employee', or 'Supplier'.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
