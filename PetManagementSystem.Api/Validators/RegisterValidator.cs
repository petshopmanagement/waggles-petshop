using FluentValidation;
using PetManagementSystem.Api.DTOs;
using System.Text.RegularExpressions;

namespace PetManagementSystem.Api.Validators;

public class RegisterValidator : AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required.")
            .Must(x => new[] { "customer", "employee", "supplier" }
            .Contains(x!.ToLower()))
            .WithMessage("Role must be Customer, Employee, or Supplier.");


        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");


        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
            .MaximumLength(20).WithMessage("Password cannot exceed 20 characters.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

        // ================= CUSTOMER & EMPLOYEE =================

        When(x => x.Role!.ToLower() == "customer" ||
                  x.Role!.ToLower() == "employee", () =>
                  {
                      RuleFor(x => x.FirstName)
                          .NotEmpty().WithMessage("First Name is required.")
                          .MaximumLength(50).WithMessage("First Name cannot exceed 50 characters.")
                          .Matches("^[a-zA-Z ]+$")
                          .WithMessage("First Name can contain only alphabets.");

                      RuleFor(x => x.LastName)
                          .NotEmpty().WithMessage("Last Name is required.")
                          .MaximumLength(50).WithMessage("Last Name cannot exceed 50 characters.")
                          .Matches("^[a-zA-Z ]+$")
                          .WithMessage("Last Name can contain only alphabets.");
                  });


        When(x => x.Role!.ToLower() == "employee", () =>
        {
            RuleFor(x => x.Position)
                .NotEmpty().WithMessage("Position is required for Employee.")
                .MaximumLength(50).WithMessage("Position cannot exceed 50 characters.");
        });


        When(x => x.Role!.ToLower() == "supplier", () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Company Name is required.")
                .MaximumLength(100).WithMessage("Company Name cannot exceed 100 characters.");

            RuleFor(x => x.ContactPerson)
                .MaximumLength(50)
                .WithMessage("Contact Person cannot exceed 50 characters.");
        });


        RuleFor(x => x.PhoneNumber)
            .Matches(@"^[0-9]{10}$")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
            .WithMessage("Phone Number must contain exactly 10 digits.");


        RuleFor(x => x.Address)
            .SetValidator(new AddressValidator()!)
            .When(x => x.Address != null);
    }
}