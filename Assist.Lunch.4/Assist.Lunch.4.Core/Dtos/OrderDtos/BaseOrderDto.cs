using Assist.Lunch._4.Domain.Enums;

namespace Assist.Lunch._4.Core.Dtos.OrderDtos
{
    public class BaseOrderDto
    {
        public IEnumerable<Guid> Foods { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public Guid UserId { get; set; }
        public Guid DestinationId { get; set; }
    }
}
