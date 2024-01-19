using Assist.Lunch._4.Core.Dtos.DestinationDtos;
using FluentValidation;

namespace Assist.Lunch._4.Core.Helpers.Validators.DestinationValidators
{
    public class BaseDestinationDtoValidator : AbstractValidator<BaseDestinationDto>
    {
        public BaseDestinationDtoValidator()
        {
            RuleFor(baseDestinationDto => baseDestinationDto.Name)
                .NotNull()
                .NotEmpty()
                .Length(3, 65);

            RuleFor(baseDestinationDto => baseDestinationDto.Address)
                .NotNull()
                .NotEmpty()
                .Length(3, 65);
        }
    }
}
