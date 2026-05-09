using FluentValidation;
using PetManagementSystem.Api.DTOs;

namespace PetManagementSystem.Api.Validators
{
    public class UpdateVaccinationValidator : AbstractValidator<UpdateVaccinationDto>
    {
        public UpdateVaccinationValidator()
        {
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .When(x => x.Price.HasValue)
                .WithMessage("Price must be greater than 0.");  
        }
    }
}