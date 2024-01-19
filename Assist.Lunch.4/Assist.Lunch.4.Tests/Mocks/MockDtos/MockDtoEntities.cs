using AutoFixture;

namespace Assist.Lunch._4.Tests.Mocks.MockDtos
{
    public static class MockDtoEntities<TEntity> where TEntity : class
    {
        public static TEntity PopulateEntity()
        {
            return new Fixture().Create<TEntity>();
        }

        public static IEnumerable<TEntity> PopulateEntityList()
        {
            var entityList = new Fixture()
                .Build<TEntity>()
                .CreateMany(4)
                .ToList();

            entityList.Add(new Fixture()
                .Build<TEntity>()
                .Create());

            return entityList;
        }
    }
}
