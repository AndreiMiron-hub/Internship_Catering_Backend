using Assist.Lunch._4.Domain.Entitites;
using AutoFixture;

namespace Assist.Lunch._4.Tests.Mocks.MockEntities
{
    public static class MockDestinationEntity
    {
        public static Destination PopulateDestination()
        {
            return new Fixture()
                .Build<Destination>()
                .Without(destination => destination.Orders)
                .Create();
        }

        public static IEnumerable<Destination> PopulateDestinationList()
        {
            return new Fixture()
                .Build<Destination>()
                .Without(destination => destination.Orders)
                .CreateMany(4)
                .ToList();
        }
    }
}
