using Assist.Lunch._4.Core.Dtos.UserDtos;
using FluentValidation;

namespace Assist.Lunch._4.Core.Helpers.Validators.UserValidators
{
    public class UpdateUserPasswordValidator : AbstractValidator<UpdateUserPasswordDto>
    {
        public UpdateUserPasswordValidator()
        {
            RuleFor(userUpdatePasswordDto => userUpdatePasswordDto.Id)
                .NotEmpty()
                .NotNull();

            RuleFor(userUpdatePasswordDto => userUpdatePasswordDto.OldPassword)
                .NotEmpty()
                .NotNull()
                .Length(8, 30)
                .Matches("[A-Z]")
                .Matches("[a-z]")
                .Matches("[0-9]")
                .Matches("[^a-zA-Z0-9]");

            RuleFor(userUpdatePasswordDto => userUpdatePasswordDto.NewPassword)
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
