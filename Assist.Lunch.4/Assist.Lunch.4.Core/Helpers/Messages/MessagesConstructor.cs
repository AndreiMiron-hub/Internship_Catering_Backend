using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Resources;
using Microsoft.AspNetCore.Http;

namespace Assist.Lunch._4.Core.Helpers.Messages
{
    public static class MessagesConstructor
    {
        public static MessageDto ReturnMessage(string message, int statusCode)
        {
            return new MessageDto() {
                Timestamp = DateTime.Now,
                Message = message,
                StatusCode = statusCode
            };
        }
    }
}
