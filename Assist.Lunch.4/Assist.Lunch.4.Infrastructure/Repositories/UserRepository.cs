using Assist.Lunch._4.Domain.Entitites;
using Assist.Lunch._4.Infrastructure.Contexts;
using Assist.Lunch._4.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Assist.Lunch._4.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await context.Users
                .Include(user => user.Orders)
                .SingleOrDefaultAsync(user => user.Email == email);

            return user;
        }
    }
}
