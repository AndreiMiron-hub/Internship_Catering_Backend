using Assist.Lunch._4.Core.Dtos.RestaurantDtos;
using FluentValidation;

namespace Assist.Lunch._4.Core.Helpers.Validators.RestaurantValidators
{
    public class RestaurantDtoValidator : AbstractValidator<RestaurantDto>
    {
        public RestaurantDtoValidator()
        {
            RuleFor(restaurantDto => restaurantDto.Id)
                .NotEmpty()
                .NotNull();

            RuleFor(restaurantDto => restaurantDto.Name)
                .NotNull()
                .NotEmpty()
                .Length(3, 60);

            RuleFor(restaurantDto => restaurantDto.IsAvailable)
                .NotNull()
                .NotEmpty();
        }
    }
}
