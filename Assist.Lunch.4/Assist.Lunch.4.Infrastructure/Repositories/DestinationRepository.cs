using Assist.Lunch._4.Domain.Entitites;
using Assist.Lunch._4.Infrastructure.Contexts;
using Assist.Lunch._4.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Assist.Lunch._4.Infrastructure.Repositories
{
    public class DestinationRepository : BaseRepository<Destination>, IDestinationRepository
    {
        public DestinationRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }

        public async Task<Destination> GetByNameAsync(string name)
        {
            var destination = await context.Destinations
                .Where(destination => !destination.IsDeleted)
                .Include(destination => destination.Orders)
                .FirstOrDefaultAsync(destination => destination.Name == name);

            return destination;
        }

        public override async Task<IEnumerable<Destination>> GetAllAsync()
        {
            List<Destination> destinations = new();

            destinations
                .AddRange(await context.Destinations
                .Where(destination => !destination.IsDeleted)
                .Include(destination => destination.Orders)
                .ToListAsync());

            return destinations;
        }

        public override async Task<Destination> GetByIdAsync(Guid destinationId)
        {
            var destination = await context.Destinations
                .Where(destination => destination.Id == destinationId)
                .Where(destination => !destination.IsDeleted)
                .SingleOrDefaultAsync();

            return destination;
        }
    }
}
