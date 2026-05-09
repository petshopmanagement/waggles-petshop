using FluentValidation;
using PetManagementSystem.Api.DTOs;

namespace PetManagementSystem.Api.Validators
{
    public class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeDto>
    {
        public UpdateEmployeeValidator()    
        {
            RuleFor(x => x.Email)
               .EmailAddress()
               .When(x => !string.IsNullOrWhiteSpace(x.Email))
               .WithMessage("Email format is invalid.");

            RuleFor(x => x.AddressId)
                .GreaterThan(0)
                .When(x => x.AddressId.HasValue)
                .WithMessage("Address Id must be greater than 0.");

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .When(x => x.FirstName != null)
                .WithMessage("First Name cannot be empty.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .When(x => x.LastName != null)
                .WithMessage("Last Name cannot be empty.");

            RuleFor(x => x.Position)
                .NotEmpty()
                .When(x => x.Position != null)
                .WithMessage("Position cannot be empty.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .When(x => x.PhoneNumber != null)
                .WithMessage("Phone Number cannot be empty.");
        }
    }
}