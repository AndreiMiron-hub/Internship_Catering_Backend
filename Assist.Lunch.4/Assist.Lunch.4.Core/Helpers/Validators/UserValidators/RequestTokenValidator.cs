using Assist.Lunch._4.Core.Models;
using FluentValidation;

namespace Assist.Lunch._4.Core.Helpers.Validators.UserValidators
{
    public class RequestTokenValidator : AbstractValidator<RequestToken>
    {
        public RequestTokenValidator()
        {
            RuleFor(requestToken => requestToken.Email)
               .NotNull()
               .NotEmpty()
               .Length(8, 65)
               .EmailAddress();

            RuleFor(requestToken => requestToken.Password)
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
