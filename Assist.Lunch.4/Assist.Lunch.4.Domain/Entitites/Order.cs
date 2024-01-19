using Assist.Lunch._4.Domain.Common;
using Assist.Lunch._4.Domain.Entitites.JoiningEntities;
using Assist.Lunch._4.Domain.Enums;

namespace Assist.Lunch._4.Domain.Entitites
{
    public record Order : BaseEntity
    {
        public int Number { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public Guid DestinationId { get; set; }
        public Destination? Destination { get; set; }
        public IEnumerable<OrderFood> OrderFoods { get; set; } = Enumerable.Empty<OrderFood>();
    }
}
