using FluentValidation;
using PetManagementSystem.Api.DTOs;

namespace PetManagementSystem.Api.Validators
{
    public class PetUpdateDTOValidator : AbstractValidator<PetUpdate>
    {
        public PetUpdateDTOValidator()
        {
            RuleFor(x => x.PetId)
                .GreaterThan(0)
                .WithMessage("Pet Id must be greater than 0.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Pet name is required.")
                .MaximumLength(100)
                .WithMessage("Pet name cannot exceed 100 characters.");

            RuleFor(x => x.Price)
                .NotNull()
                .WithMessage("Price is required.")
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0.");

            RuleFor(x => x.Age)
                .NotNull()
                .WithMessage("Age is required.")
                .InclusiveBetween(0, 50)
                .WithMessage("Age must be between 0 and 50.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.Breed)
                .NotEmpty()
                .WithMessage("Breed is required.")
                .MaximumLength(100)
                .WithMessage("Breed cannot exceed 100 characters.");

            RuleFor(x => x.ImageUrl)
                .MaximumLength(500)
                .WithMessage("Image URL cannot exceed 500 characters.");
        }
    }
}