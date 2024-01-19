using Assist.Lunch._4.Domain.Entitites;

namespace Assist.Lunch._4.Infrastructure.Interfaces
{
    public interface IRestaurantRepository : IBaseRepository<Restaurant>
    {
        Task<Restaurant> GetByNameAsync(string name);
    }
}
