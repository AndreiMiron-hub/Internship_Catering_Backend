using Assist.Lunch._4.Domain.Common;
using Assist.Lunch._4.Domain.Configurations;
using Assist.Lunch._4.Domain.Entitites;
using Assist.Lunch._4.Domain.Entitites.JoiningEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace Assist.Lunch._4.Infrastructure.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IHttpContextAccessor httpContext;

        public ApplicationDbContext(DbContextOptions options, IHttpContextAccessor httpContext) : base(options)
        {
            this.httpContext = httpContext;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<OrderFood> OrderFoods { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.Now;
                        entry.Entity.CreatedBy = Guid.Parse(
                            new JwtSecurityTokenHandler()
                            .ReadJwtToken(httpContext.HttpContext.Request.Headers["Authorization"]
                            .ToString().Split(" ").Last())
                            .Claims.First(claim => claim.Type == "id").Value);
                        break;

                    case EntityState.Modified:
                        entry.Entity.ModifiedAt = DateTime.Now;
                        entry.Entity.ModifiedBy = Guid.Parse(
                            new JwtSecurityTokenHandler()
                            .ReadJwtToken(httpContext.HttpContext.Request.Headers["Authorization"]
                            .ToString().Split(" ").Last())
                            .Claims.First(claim => claim.Type == "id").Value);
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new DestinationConfiguration());
            modelBuilder.ApplyConfiguration(new RestaurantConfiguration());
            modelBuilder.ApplyConfiguration(new FoodConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderFoodConfiguration());
        }
    }
}
