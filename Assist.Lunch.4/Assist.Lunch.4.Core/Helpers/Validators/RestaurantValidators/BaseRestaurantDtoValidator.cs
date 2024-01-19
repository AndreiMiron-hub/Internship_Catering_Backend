using Assist.Lunch._4.Core.Dtos.RestaurantDtos;
using FluentValidation;

namespace Assist.Lunch._4.Core.Helpers.Validators.RestaurantValidators
{
    public class BaseRestaurantDtoValidator : AbstractValidator<BaseRestaurantDto>
    {
        public BaseRestaurantDtoValidator()
        {
            RuleFor(baseRestaurantDto => baseRestaurantDto.Name)
               .NotNull()
               .NotEmpty()
               .Length(3, 60);

            RuleFor(baseRestaurantDto => baseRestaurantDto.IsAvailable)
                .NotNull()
                .NotEmpty();
        }
    }
}
