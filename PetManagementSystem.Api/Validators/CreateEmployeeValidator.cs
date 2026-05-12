using FluentValidation;
using PetManagementSystem.Api.DTOs;

namespace PetManagementSystem.Api.Validators
{
    public class CreateEmployeeValidator : AbstractValidator<WriteEmployeeDto>
    {
        public CreateEmployeeValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First Name is required.")
                .MaximumLength(50)
                .WithMessage("First Name cannot exceed 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last Name is required.")
                .MaximumLength(50)
                .WithMessage("Last Name cannot exceed 50 characters.");

            RuleFor(x => x.Position)
                .NotEmpty()
                .WithMessage("Position is required.")
                .MaximumLength(100)
                .WithMessage("Position cannot exceed 100 characters.");

            RuleFor(x => x.HireDate)
                .NotNull()
                .WithMessage("Hire Date is required.")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("Hire Date cannot be in the future.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("Phone Number is required.")
                .Matches(@"^[0-9]{10}$")
                .WithMessage("Phone Number must contain exactly 10 digits.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("A valid Email is required.")
                .MaximumLength(100)
                .WithMessage("Email cannot exceed 100 characters.");

            RuleFor(x => x.Address)
                .NotNull()
                .WithMessage("Address is required.");
        }
    }
}