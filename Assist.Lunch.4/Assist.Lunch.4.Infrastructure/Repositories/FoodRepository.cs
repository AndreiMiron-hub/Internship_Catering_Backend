using Assist.Lunch._4.Domain.Entitites;
using Assist.Lunch._4.Infrastructure.Contexts;
using Assist.Lunch._4.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Assist.Lunch._4.Infrastructure.Repositories
{
    public class FoodRepository : BaseRepository<Food>, IFoodRepository
    {
        public FoodRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }

        public override async Task<IEnumerable<Food>> GetAllAsync()
        {
            List<Food> foods = new();

            foods
                .AddRange(await context.Foods
                .Where(food => !food.IsDeleted)
                .Include(food => food.Restaurant)
                .ToListAsync());

            return foods;
        }

        public async Task<IEnumerable<Food>> GetAllAsync(IEnumerable<Guid> foodIds)
        {
            List<Food> foods = new();

            foods
                .AddRange(await context.Foods
                .Where(food => !food.IsDeleted)
                .Where(food => foodIds.Contains(food.Id))
                .Include(food => food.Restaurant)
                .ToListAsync());

            return foods;
        }

        public override async Task<Food> GetByIdAsync(Guid foodId)
        {
            Food foods = new();

            foods = await context.Foods
                .Where(food => food.Id == foodId)
                .Where(food => !food.IsDeleted)
                .Include(food => food.Restaurant)
                .FirstOrDefaultAsync();

            return foods;
        }

        public async Task<IEnumerable<Food>> GetByRestaurantAsync(Guid restaurantId)
        {
            List<Food> restaurantFoods = new();

            restaurantFoods
                .AddRange(await context.Foods
                .Where(food => food.RestaurantId == restaurantId)
                .Where(food => !food.IsDeleted)
                .Include(food => food.Restaurant)
                .ToListAsync());

            return restaurantFoods;
        }

        public async Task<Food> GetByRestaurantAndNameAsync(Guid restaurantId, string foodName)
        {
            Food? restaurantFood = new();

            restaurantFood = await context.Foods
                .Where(food => food.RestaurantId == restaurantId)
                .Where(food => food.Name == foodName)
                .Where(food => !food.IsDeleted)
                .Include(food => food.Restaurant)
                .FirstOrDefaultAsync();

            return restaurantFood;
        }
    }
}
