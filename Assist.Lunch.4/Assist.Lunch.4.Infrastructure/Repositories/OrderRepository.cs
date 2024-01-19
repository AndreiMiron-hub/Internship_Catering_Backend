using Assist.Lunch._4.Domain.Entitites;
using Assist.Lunch._4.Domain.Entitites.JoiningEntities;
using Assist.Lunch._4.Infrastructure.Contexts;
using Assist.Lunch._4.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Assist.Lunch._4.Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }

        public async Task<IEnumerable<Order>> GetAllActiveAsync()
        {
            var orders = await context.Orders
                .Where(order => !order.IsDeleted)
                .Select(order => new Order
                {
                    Id = order.Id,
                    Number = order.Number,
                    TimeSlot = order.TimeSlot,
                    CreatedAt = order.CreatedAt,
                    ModifiedAt = order.ModifiedAt,
                    CreatedBy = order.CreatedBy,
                    ModifiedBy = order.ModifiedBy,

                    User = new User
                    {
                        FirstName = order.User.FirstName,
                        LastName = order.User.LastName,
                        Email = order.User.Email,
                        Id = order.User.Id
                    },

                    Destination = new Destination
                    {
                        Name = order.Destination.Name,
                        Address = order.Destination.Address,
                        Id = order.Destination.Id
                    },

                    OrderFoods = new List<OrderFood>
                    {
                        new OrderFood
                        {
                            Food = order.OrderFoods
                            .OrderByDescending(orderFood => orderFood.OrderId)
                            .Select(orderFood => new Food
                            {
                                Id = orderFood.Food.Id,
                                Name = orderFood.Food.Name,
                                Category = orderFood.Food.Category,
                                Price = orderFood.Food.Price,
                                Restaurant = new Restaurant
                                {
                                    Name = orderFood.Food.Restaurant.Name,
                                    IsAvailable = orderFood.Food.Restaurant.IsAvailable,
                                    Id = orderFood.Food.Restaurant.Id
                                }
                            }).FirstOrDefault()
                        }
                    }
                }).ToListAsync();

            return orders;
        }

        public override async Task<Order> GetByIdAsync(Guid id)
        {
            var order = await context.Orders
               .Where(order => !order.IsDeleted)
               .Where(order => order.Id == id)
               .Select(order => new Order
               {
                   Id = order.Id,
                   Number = order.Number,
                   TimeSlot = order.TimeSlot,
                   CreatedAt = order.CreatedAt,
                   ModifiedAt = order.ModifiedAt,
                   CreatedBy = order.CreatedBy,
                   ModifiedBy = order.ModifiedBy,

                   User = new User
                   {
                       FirstName = order.User.FirstName,
                       LastName = order.User.LastName,
                       Email = order.User.Email,
                       Id = order.User.Id
                   },

                   Destination = new Destination
                   {
                       Name = order.Destination.Name,
                       Address = order.Destination.Address,
                       Id = order.Destination.Id
                   },

                   OrderFoods = new List<OrderFood>
                   {
                        new OrderFood
                        {
                            Food = order.OrderFoods
                            .OrderByDescending(of => of.OrderId)
                            .Select(orderFood => new Food
                            {
                                Id = orderFood.Food.Id,
                                Name = orderFood.Food.Name,
                                Category = orderFood.Food.Category,
                                Price = orderFood.Food.Price,
                                Restaurant = new Restaurant
                                {
                                    Name = orderFood.Food.Restaurant.Name,
                                    IsAvailable = orderFood.Food.Restaurant.IsAvailable,
                                    Id = orderFood.Food.Restaurant.Id
                                }
                            }).FirstOrDefault()
                        }
                   }
               }).SingleOrDefaultAsync();

            return order;
        }

        public async Task<IEnumerable<Order>> GetTodaysOrdersAsync()
        {
            return await context.Orders
                .Where(order => order.CreatedAt.Day == DateTime.Today.Day)
                .Select(order => new Order
                {
                    Id = order.Id,
                    Number = order.Number,
                    TimeSlot = order.TimeSlot,
                    CreatedAt = order.CreatedAt,
                    ModifiedAt = order.ModifiedAt,
                    CreatedBy = order.CreatedBy,
                    ModifiedBy = order.ModifiedBy,

                    User = new User
                    {
                        FirstName = order.User.FirstName,
                        LastName = order.User.LastName,
                        Email = order.User.Email,
                        Id = order.User.Id
                    },

                    Destination = new Destination
                    {
                        Name = order.Destination.Name,
                        Address = order.Destination.Address,
                        Id = order.Destination.Id
                    },

                    OrderFoods = new List<OrderFood>
                    {
                        new OrderFood
                        {
                            Food = order.OrderFoods
                            .OrderByDescending(of => of.OrderId)
                            .Select(orderFood => new Food
                            {
                                Id = orderFood.Food.Id,
                                Name = orderFood.Food.Name,
                                Category = orderFood.Food.Category,
                                Price = orderFood.Food.Price,
                                Restaurant = new Restaurant
                                {
                                    Name = orderFood.Food.Restaurant.Name,
                                    IsAvailable = orderFood.Food.Restaurant.IsAvailable,
                                    Id = orderFood.Food.Restaurant.Id
                                }
                            }).FirstOrDefault()
                        }
                    }
                }).ToListAsync();
        }

        public async Task<Order> GetTodaysOrderByUserAsync(Guid userId)
        {
            var order = await context.Orders
                .Where(order => order.UserId == userId)
                .Where(order => order.CreatedAt.Day == DateTime.Today.Day)
                .Select(order => new Order
                {
                    Id = order.Id,
                    Number = order.Number,
                    TimeSlot = order.TimeSlot,
                    CreatedAt = order.CreatedAt,
                    ModifiedAt = order.ModifiedAt,
                    CreatedBy = order.CreatedBy,
                    ModifiedBy = order.ModifiedBy,

                    User = new User
                    {
                        FirstName = order.User.FirstName,
                        LastName = order.User.LastName,
                        Email = order.User.Email,
                        Id = order.User.Id
                    },

                    Destination = new Destination
                    {
                        Name = order.Destination.Name,
                        Address = order.Destination.Address,
                        Id = order.Destination.Id
                    },

                    OrderFoods = new List<OrderFood>
                    {
                        new OrderFood
                        {
                            Food = order.OrderFoods
                            .OrderByDescending(of => of.OrderId)
                            .Select(orderFood => new Food
                            {
                                Id = orderFood.Food.Id,
                                Name = orderFood.Food.Name,
                                Category = orderFood.Food.Category,
                                Price = orderFood.Food.Price,
                                Restaurant = new Restaurant
                                {
                                    Name = orderFood.Food.Restaurant.Name,
                                    IsAvailable = orderFood.Food.Restaurant.IsAvailable,
                                    Id = orderFood.Food.Restaurant.Id
                                }
                            }).FirstOrDefault()
                        }
                    }
                }).SingleOrDefaultAsync();

            return order;
        }

        public async Task<Order> GetTodaysOrderAsync(Guid userId)
        {
            var order = await context.Orders
                .Where(order => order.UserId == userId)
                .Where(order => order.CreatedAt.Day == DateTime.Today.Day)
                .Include(order => order.Destination)
                .Include(order => order.User)
                .Include(order => order.OrderFoods)
                .ThenInclude(orderFoods => orderFoods.Food)
                .ThenInclude(food => food.Restaurant)
                .SingleOrDefaultAsync();

            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserAsync(Guid userId, DateTime? startDate, DateTime? endDate)
        {
            if (startDate is not null && endDate is not null)
            {
                var filteredOrders = await context.Orders
                .Where(order => order.UserId == userId)
                .Where(order =>
                    order.CreatedAt.Day <= endDate.Value.Day &&
                    order.CreatedAt.Day >= startDate.Value.Day)
                .Select(s => new Order
                {
                    Id = s.Id,
                    Number = s.Number,
                    TimeSlot = s.TimeSlot,

                    User = new User
                    {
                        FirstName = s.User.FirstName,
                        LastName = s.User.LastName,
                        Email = s.User.Email,
                        Id = s.User.Id
                    },

                    Destination = new Destination
                    {
                        Name = s.Destination.Name,
                        Address = s.Destination.Address,
                        Id = s.Destination.Id
                    },

                    OrderFoods = new List<OrderFood>
                    {
                        new OrderFood
                        {
                            Food = s.OrderFoods
                            .OrderByDescending(of => of.OrderId)
                            .Select(ss => new Food
                            {
                                Id = ss.Food.Id,
                                Name = ss.Food.Name,
                                Category = ss.Food.Category,
                                Price = ss.Food.Price,
                                Restaurant = new Restaurant
                                {
                                    Name = ss.Food.Restaurant.Name,
                                    IsAvailable = ss.Food.Restaurant.IsAvailable,
                                    Id = ss.Food.Restaurant.Id
                                }
                            }).FirstOrDefault()
                        }
                    }
                })
                .ToListAsync();

                return filteredOrders;
            }

            var orders = await context.Orders
                 .Where(order => order.UserId == userId)
                 .Select(order => new Order
                 {
                     Id = order.Id,
                     Number = order.Number,
                     TimeSlot = order.TimeSlot,
                     CreatedAt = order.CreatedAt,
                     ModifiedAt = order.ModifiedAt,
                     CreatedBy = order.CreatedBy,
                     ModifiedBy = order.ModifiedBy,

                     User = new User
                     {
                         FirstName = order.User.FirstName,
                         LastName = order.User.LastName,
                         Email = order.User.Email,
                         Id = order.User.Id
                     },

                     Destination = new Destination
                     {
                         Name = order.Destination.Name,
                         Address = order.Destination.Address,
                         Id = order.Destination.Id
                     },

                     OrderFoods = new List<OrderFood>
                    {
                        new OrderFood
                        {
                            Food = order.OrderFoods
                            .OrderByDescending(of => of.OrderId)
                            .Select(orderFood => new Food
                            {
                                Id = orderFood.Food.Id,
                                Name = orderFood.Food.Name,
                                Category = orderFood.Food.Category,
                                Price = orderFood.Food.Price,
                                Restaurant = new Restaurant
                                {
                                    Name = orderFood.Food.Restaurant.Name,
                                    IsAvailable = orderFood.Food.Restaurant.IsAvailable,
                                    Id = orderFood.Food.Restaurant.Id
                                }
                            }).FirstOrDefault()
                        }
                    }
                 }).ToListAsync();

            return orders;
        }
    }
}
