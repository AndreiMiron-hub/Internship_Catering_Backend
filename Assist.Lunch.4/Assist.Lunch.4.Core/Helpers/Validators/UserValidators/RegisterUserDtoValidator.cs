using Assist.Lunch._4.Core.Dtos.UserDtos;
using FluentValidation;

namespace Assist.Lunch._4.Core.Helpers.Validators.UserValidators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator()
        {
            RuleFor(registerUserDto => registerUserDto.FirstName)
               .NotNull()
               .NotEmpty()
               .Length(3, 30);

            RuleFor(registerUserDto => registerUserDto.LastName)
                .NotNull()
                .NotEmpty()
                .Length(3, 30);

            RuleFor(registerUserDto => registerUserDto.Email)
                .NotNull()
                .NotEmpty()
                .Length(8, 65)
                .EmailAddress();

            RuleFor(registerUserDto => registerUserDto.Password)
                .NotEmpty()
                .NotNull()
                .Length(8, 30)
                .Matches("[A-Z]")
                .Matches("[a-z]")
                .Matches("[0-9]")
                .Matches("[^a-zA-Z0-9]");
        }
    }
}
