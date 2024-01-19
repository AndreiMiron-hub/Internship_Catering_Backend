using Assist.Lunch._4.Domain.Entitites;

namespace Assist.Lunch._4.Infrastructure.Interfaces
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<IEnumerable<Order>> GetAllActiveAsync();
        Task<IEnumerable<Order>> GetOrdersByUserAsync(Guid userId, DateTime? sratDate, DateTime? endDate);
        Task<IEnumerable<Order>> GetTodaysOrdersAsync();
        Task<Order> GetTodaysOrderAsync(Guid userId);
        Task<Order> GetTodaysOrderByUserAsync(Guid userId);
    }
}
