using FluentValidation;
using PetManagementSystem.Api.DTOs.GroomingServiceDtos;

namespace PetManagementSystem.Api.Validators
{
    public class CreateGroomingServiceDtoValidator : AbstractValidator<CreateGroomingServiceDto>
    {
        public CreateGroomingServiceDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Service name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.")
                .When(x => x.Price.HasValue);

        
        }
    }
}
