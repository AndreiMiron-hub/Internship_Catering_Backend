using Assist.Lunch._4.Core.Dtos.FoodDtos;
using FluentValidation;

namespace Assist.Lunch._4.Core.Helpers.Validators.FoodValidators
{
    public class AddFoodDtoValidator : AbstractValidator<AddFoodDto>
    {
        public AddFoodDtoValidator()
        {
            RuleFor(addFoodDto => addFoodDto.RestaurantId)
                .NotNull()
                .NotEmpty();

            RuleFor(addFoodDto => addFoodDto.Name)
                .NotNull()
                .NotEmpty()
                .Length(3, 65);

            RuleFor(addFoodDto => addFoodDto.Price)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0);

            RuleFor(addFoodDto => addFoodDto.Category)
                .NotNull()
                .NotEmpty();
        }
    }
}
