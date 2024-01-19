using System.Globalization;

namespace Assist.Lunch._4.Core.Helpers.ExceptionHandler.CustomExceptions
{
    public class EntityAlreadyExistsException : Exception
    {
        public EntityAlreadyExistsException() : base() { }

        public EntityAlreadyExistsException(string message) : base(message) { }

        public EntityAlreadyExistsException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args)) { }
    }
}
