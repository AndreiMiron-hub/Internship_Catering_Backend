using Assist.Lunch._4.Core.Dtos.DestinationDtos;
using FluentValidation;

namespace Assist.Lunch._4.Core.Helpers.Validators.DestinationValidators
{
    public class DestinationDtoValidator : AbstractValidator<DestinationDto>
    {
        public DestinationDtoValidator()
        {
            RuleFor(destinationDto => destinationDto.Id)
                .NotNull()
                .NotEmpty();

            RuleFor(destinationDto => destinationDto.Name)
                .NotNull()
                .NotEmpty()
                .Length(3, 65);

            RuleFor(destinationDto => destinationDto.Address)
                .NotNull()
                .NotEmpty()
                .Length(3, 65);
        }
    }
}
