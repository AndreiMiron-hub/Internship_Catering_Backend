using Assist.Lunch._4.Domain.Entitites;
using AutoFixture;

namespace Assist.Lunch._4.Tests.Mocks.MockEntities
{
    public static class MockFoodEntity
    {
        public static Food PopulateFood()
        {
            return new Fixture()
                .Build<Food>()
                .Without(food => food.Restaurant)
                .Without(food => food.OrderFoods)
                .Create();
        }

        public static IEnumerable<Food> PopulateFoodList()
        {
            return new Fixture()
                .Build<Food>()
                .Without(food => food.Restaurant)
                .Without(food => food.OrderFoods)
                .CreateMany(4)
                .ToList();
        }
    }
}
