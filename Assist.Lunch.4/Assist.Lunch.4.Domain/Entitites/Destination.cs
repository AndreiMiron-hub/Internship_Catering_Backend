using Assist.Lunch._4.Domain.Common;

namespace Assist.Lunch._4.Domain.Entitites
{
    public record Destination : BaseEntity
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
