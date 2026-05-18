using FluentValidation;
using PetManagementSystem.Api.DTOs;

namespace PetManagementSystem.Api.Validators
{
    public class CreateVaccinationValidator : AbstractValidator<WriteVaccinationDto>
    {
        public CreateVaccinationValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Vaccination Name is required.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required.");

            RuleFor(x => x.Price)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0.");

            RuleFor(x => x.Available)
                .NotNull()
                .WithMessage("Available field is required.");
        }
    }
}