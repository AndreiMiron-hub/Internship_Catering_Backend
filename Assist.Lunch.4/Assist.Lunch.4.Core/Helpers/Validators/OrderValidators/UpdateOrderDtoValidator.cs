using Assist.Lunch._4.Core.Dtos.OrderDtos;
using FluentValidation;

namespace Assist.Lunch._4.Core.Helpers.Validators.OrderValidators
{
    public class UpdateOrderDtoValidator : AbstractValidator<UpdateOrderDto>
    {
        public UpdateOrderDtoValidator()
        {
            RuleFor(updateOrderDto => updateOrderDto.Id)
                .NotNull()
                .NotEmpty();
        }
    }
}
