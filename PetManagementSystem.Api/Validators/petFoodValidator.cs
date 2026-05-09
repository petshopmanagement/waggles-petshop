using PetManagementSystem.Api.DTOs;
using FluentValidation;

namespace PetManagementSystem.Api.Validators
{
    public class CreatePetFoodDto : AbstractValidator<FoodDTO>
    {
        public CreatePetFoodDto()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Food name is required")
                .MaximumLength(100)
                .WithMessage("Food name cannot exceed 100 characters");

            RuleFor(x => x.Brand)
                .NotEmpty()
                .WithMessage("Brand is required")
                .MaximumLength(100)
                .WithMessage("Brand cannot exceed 100 characters");

            RuleFor(x => x.Type)
                .NotEmpty()
                .WithMessage("Food type is required")
                .MaximumLength(50)
                .WithMessage("Food type cannot exceed 50 characters");

            RuleFor(x => x.Quantity)
                .NotNull()
                .WithMessage("Quantity is required")
                .GreaterThanOrEqualTo(0)
                .WithMessage("Quantity cannot be negative");

            RuleFor(x => x.Price)
                .NotNull()
                .WithMessage("Price is required")
                .GreaterThan(0)
                .WithMessage("Price must be greater than zero");
        }
    }

    // Validator for Update DTO
    public class UpdatePetFoodDtoValidator
        : AbstractValidator<UpdatePetFoodDto>
    {
        public UpdatePetFoodDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Food name is required")
                .MaximumLength(100);

            RuleFor(x => x.Brand)
                .NotEmpty()
                .WithMessage("Brand is required")
                .MaximumLength(100);

            RuleFor(x => x.Type)
                .NotEmpty()
                .WithMessage("Food type is required")
                .MaximumLength(50);

            RuleFor(x => x.Quantity)
                .NotNull()
                .WithMessage("Quantity is required")
                .GreaterThanOrEqualTo(0)
                .WithMessage("Quantity cannot be negative");

            RuleFor(x => x.Price)
                .NotNull()
                .WithMessage("Price is required")
                .GreaterThan(0)
                .WithMessage("Price must be greater than zero");
        }
    
}
}
