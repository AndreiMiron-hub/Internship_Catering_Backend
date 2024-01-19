using Assist.Lunch._4.Domain.Entitites;

namespace Assist.Lunch._4.Infrastructure.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
    }
}
