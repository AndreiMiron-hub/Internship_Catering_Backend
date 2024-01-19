using Assist.Lunch._4.Domain.Entitites;
using AutoFixture;
using AutoFixture.Kernel;
using System.Collections.Generic;

namespace Assist.Lunch._4.Tests.Mocks.MockEntities
{
    public static class MockOrderEntity
    {
        public static Order PopulateOrder()
        {
            return new Fixture()
                .Build<Order>()
                .Without(order => order.OrderFoods)
                .Without(order => order.Destination)
                .Without(order => order.User)
                .Create();
        }

        public static IEnumerable<Order> PopulateOrderList()
        {
            return new Fixture()
                .Build<Order>()
                .Without(order => order.OrderFoods)
                .Without(order => order.Destination)
                .Without(Order => Order.User)
                .CreateMany(4)
                .ToList();
        }
    }
}
