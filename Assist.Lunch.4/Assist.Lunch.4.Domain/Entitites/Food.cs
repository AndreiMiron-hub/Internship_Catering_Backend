using Assist.Lunch._4.Domain.Common;
using Assist.Lunch._4.Domain.Entitites.JoiningEntities;
using Assist.Lunch._4.Domain.Enums;

namespace Assist.Lunch._4.Domain.Entitites
{
    public record Food : BaseEntity
    {
        public string? Name { get; set; }
        public Category Category { get; set; }
        public double Price { get; set; }
        public Guid RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }
        public ICollection<OrderFood>? OrderFoods { get; set; }
    }
}
