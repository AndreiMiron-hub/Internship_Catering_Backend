using Assist.Lunch._4.Core.Dtos.FoodDtos;
using FluentValidation;

namespace Assist.Lunch._4.Core.Helpers.Validators.FoodValidators
{
    public class UpdateFoodDtoValidator : AbstractValidator<UpdateFoodDto>
    {
        public UpdateFoodDtoValidator()
        {
            RuleFor(updateFoodDto => updateFoodDto.Id)
               .NotNull()
               .NotEmpty();

            RuleFor(updateFoodDto => updateFoodDto.Name)
                .NotNull()
                .NotEmpty()
                .Length(3, 65);

            RuleFor(updateFoodDto => updateFoodDto.Price)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0);

            RuleFor(addFoodDto => addFoodDto.Category)
                .NotNull()
                .NotEmpty();
        }
    }
}
