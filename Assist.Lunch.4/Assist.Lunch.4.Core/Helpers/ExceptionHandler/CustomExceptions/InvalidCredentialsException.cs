using System.Globalization;

namespace Assist.Lunch._4.Core.Helpers.ExceptionHandler.CustomExceptions
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException() : base() { }

        public InvalidCredentialsException(string message) : base(message) { }

        public InvalidCredentialsException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args)) { }
    }
}
