using Assist.Lunch._4.Domain.Entitites;
using AutoFixture;

namespace Assist.Lunch._4.Tests.Mocks.MockEntities
{
    public static class MockUserEntity
    {
        public static User PopulateUser()
        {
            return new Fixture()
                .Build<User>()
                .Without(user => user.Orders)
                .Create();
        }

        public static IEnumerable<User> PopulateUserList()
        {
            return new Fixture()
                .Build<User>()
                .Without(user => user.Orders)
                .CreateMany(4)
                .ToList();
        }
    }
}
