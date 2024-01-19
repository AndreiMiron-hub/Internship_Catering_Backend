namespace Assist.Lunch._4.Domain.Entitites.JoiningEntities
{
    public record OrderFood
    {
        public Guid FoodId { get; set; }
        public Guid OrderId { get; set; }
        public Food? Food { get; set; }
        public Order? Order { get; set; }
    }
}
