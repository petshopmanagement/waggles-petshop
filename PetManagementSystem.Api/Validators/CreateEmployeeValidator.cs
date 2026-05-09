using FluentValidation;
using PetManagementSystem.Api.DTOs;

namespace PetManagementSystem.Api.Validators
{
    public class CreateEmployeeValidator : AbstractValidator<CreateEmployeeDto>
    {
            public CreateEmployeeValidator()
            {
                RuleFor(x => x.FirstName)
                    .NotEmpty()
                    .WithMessage("First Name is required.");

                RuleFor(x => x.LastName)
                    .NotEmpty()
                    .WithMessage("Last Name is required.");

                RuleFor(x => x.Position)
                    .NotEmpty()
                    .WithMessage("Position is required.");

                RuleFor(x => x.PhoneNumber)
                    .NotEmpty()
                    .WithMessage("Phone Number is required.");

                RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .WithMessage("A valid Email is required.");

                RuleFor(x => x.AddressId)
                    .NotNull()
                    .WithMessage("Address Id is required.");

            }
        }
    }

