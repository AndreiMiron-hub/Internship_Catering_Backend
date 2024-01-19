using Assist.Lunch._4.Core.Dtos.OrderDtos;
using FluentValidation;

namespace Assist.Lunch._4.Core.Helpers.Validators.OrderValidators
{
    public class BaseOrderDtoValidator : AbstractValidator<BaseOrderDto>
    {
        public BaseOrderDtoValidator()
        {
            RuleFor(baseOrderDto => baseOrderDto.Foods)
                .NotNull()
                .NotEmpty();

            RuleFor(baseOrderDto => baseOrderDto.TimeSlot)
                .NotNull()
                .NotEmpty();

            RuleFor(baseOrderDto => baseOrderDto.UserId)
                .NotEmpty()
                .NotNull();

            RuleFor(addOrderDto => addOrderDto.DestinationId)
                .NotNull()
                .NotEmpty();
        }
    }
}
