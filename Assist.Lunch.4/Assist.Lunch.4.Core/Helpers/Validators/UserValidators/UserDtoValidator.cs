using Assist.Lunch._4.Core.Dtos.UserDtos;
using FluentValidation;

namespace Assist.Lunch._4.Core.Helpers.Validators.UserValidators
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(userDto => userDto.Id)
                .NotNull()
                .NotEmpty();

            RuleFor(userDto => userDto.FirstName)
               .NotNull()
               .NotEmpty()
               .Length(3, 30);

            RuleFor(userDto => userDto.LastName)
                .NotNull()
                .NotEmpty()
                .Length(3, 30);

            RuleFor(userDto => userDto.Email)
                .NotNull()
                .NotEmpty()
                .Length(8, 65)
                .EmailAddress();
        }
    }
}
