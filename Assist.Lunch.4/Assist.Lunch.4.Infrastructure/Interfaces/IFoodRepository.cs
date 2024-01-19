using Assist.Lunch._4.Domain.Entitites;

namespace Assist.Lunch._4.Infrastructure.Interfaces
{
    public interface IFoodRepository : IBaseRepository<Food>
    {
        Task<IEnumerable<Food>> GetAllAsync(IEnumerable<Guid> foodIds);
        Task<IEnumerable<Food>> GetByRestaurantAsync(Guid restaurantId);
        Task<Food> GetByRestaurantAndNameAsync(Guid restaurantId, string foodName);
    }
}
