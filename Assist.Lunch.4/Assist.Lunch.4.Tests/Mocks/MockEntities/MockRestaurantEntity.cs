using Assist.Lunch._4.Domain.Entitites;
using AutoFixture;

namespace Assist.Lunch._4.Tests.Mocks.MockEntities
{
    public static class MockRestaurantEntity
    {
        public static Restaurant PopulateRestaurant()
        {
            return new Fixture()
                .Build<Restaurant>()
                .Without(restaurant => restaurant.Foods)
                .Create();
        }

        public static IEnumerable<Restaurant> PopulateRestaurantList()
        {
            return new Fixture()
                .Build<Restaurant>()
                .Without(restaurant => restaurant.Foods)
                .CreateMany(4)
                .ToList();
        }
    }
}
