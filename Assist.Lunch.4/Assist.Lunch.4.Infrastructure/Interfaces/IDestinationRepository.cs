using Assist.Lunch._4.Domain.Entitites;

namespace Assist.Lunch._4.Infrastructure.Interfaces
{
    public interface IDestinationRepository : IBaseRepository<Destination>
    {
        Task<Destination> GetByNameAsync(string name);
    }
}
