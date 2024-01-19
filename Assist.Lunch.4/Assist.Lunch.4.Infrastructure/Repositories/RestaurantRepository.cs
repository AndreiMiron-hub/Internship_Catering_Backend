using Assist.Lunch._4.Domain.Entitites;
using Assist.Lunch._4.Infrastructure.Contexts;
using Assist.Lunch._4.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Assist.Lunch._4.Infrastructure.Repositories
{
    public class RestaurantRepository : BaseRepository<Restaurant>, IRestaurantRepository
    {
        public RestaurantRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }

        public override async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            List<Restaurant> restaurants = new();

            restaurants
                .AddRange(await context.Restaurants
                .Where(restaurant => !restaurant.IsDeleted)
                .Include(restaurant => restaurant.Foods)
                .ToListAsync());

            return restaurants;
        }

        public async Task<Restaurant> GetByNameAsync(string name)
        {
            var restaurant = await context.Restaurants
                .Include(restaurant => restaurant.Foods)
                .FirstOrDefaultAsync(restaurant => restaurant.Name == name);

            return restaurant;
        }
    }
}
