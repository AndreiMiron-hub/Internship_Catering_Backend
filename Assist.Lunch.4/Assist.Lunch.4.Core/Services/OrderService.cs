using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.FoodDtos;
using Assist.Lunch._4.Core.Dtos.OrderDtos;
using Assist.Lunch._4.Core.Dtos.UserDtos;
using Assist.Lunch._4.Core.Helpers.ExceptionHandler.CustomExceptions;
using Assist.Lunch._4.Core.Helpers.Messages;
using Assist.Lunch._4.Core.Interfaces;
using Assist.Lunch._4.Core.Security.Utils;
using Assist.Lunch._4.Domain.Entitites;
using Assist.Lunch._4.Domain.Entitites.JoiningEntities;
using Assist.Lunch._4.Domain.Enums;
using Assist.Lunch._4.Infrastructure.Interfaces;
using Assist.Lunch._4.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace Assist.Lunch._4.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IUserRepository userRepository;
        private readonly IFoodRepository foodRepository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContext;
        private readonly IJwtUtils jwtUtils;

        public OrderService(IOrderRepository orderRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IFoodRepository foodRepository,
            IHttpContextAccessor httpContext,
            IJwtUtils jwtUtils)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.foodRepository = foodRepository;
            this.httpContext = httpContext;
            this.jwtUtils = jwtUtils;
        }

        public async Task<ResponseOrderDto> Add(BaseOrderDto baseOrderDto)
        {
            //jwtUtils.CheckForActiveUser(httpContext, baseOrderDto.UserId);

            var todaysOrders = await orderRepository.GetTodaysOrdersAsync();

            if (todaysOrders.Any(todaysOrder => todaysOrder.User.Id == baseOrderDto.UserId))
            {
                throw new EntityAlreadyExistsException(OrderResources.UserHasAlreadyOrdered);
            }

            var newOrder = mapper.Map<Order>(baseOrderDto);

            newOrder.Number = todaysOrders.Count() + 1;

            var foodsList = await foodRepository.GetAllAsync(baseOrderDto.Foods);

            newOrder.OrderFoods
                .ToList()
                .AddRange(foodsList
                .Select(of => new OrderFood() { Food = of, Order = newOrder }));

            newOrder = await orderRepository.InsertAsync(newOrder);

            return await GetById(newOrder.Id);
        }

        public async Task<MessageDto> Delete(Guid userId)
        {
            jwtUtils.CheckForOwnership(httpContext, userId);

            var order = await orderRepository.GetTodaysOrderByUserAsync(userId);

            if (order is null)
            {
                throw new KeyNotFoundException(OrderResources.NotFound);
            }

            await orderRepository.DeleteAsync(order);

            return MessagesConstructor.ReturnMessage(OrderResources.OrderDeleted,
                StatusCodes.Status200OK);
        }

        public async Task<ResponseDto<ResponseOrderDto>> GetAll()
        {
            var orders = await orderRepository.GetAllActiveAsync();

            return MapResponseOrderDto(orders);
        }

        public async Task<ResponseOrderDto> GetById(Guid orderId)
        {
            var order = await orderRepository.GetByIdAsync(orderId);

            if (order is null)
            {
                throw new KeyNotFoundException(OrderResources.NotFound);
            }

            return MapResponseOrderDto(order);
        }

        public async Task<ResponseDto<ResponseOrderDto>> GetByUser(Guid userId)
        {
            var orders = await orderRepository.GetOrdersByUserAsync(userId, null, null);

            if (!orders.Any())
            {
                throw new KeyNotFoundException(OrderResources.NotFound);
            }

            return MapResponseOrderDto(orders);
        }

        public async Task<ResponseOrderHistoryDto> GetHistoryByUser(RequestOrderHistoryDto requestOrderHistoryDto)
        {
            jwtUtils.CheckForOwnership(httpContext, requestOrderHistoryDto.UserId);

            var orders = await orderRepository.GetOrdersByUserAsync(
                requestOrderHistoryDto.UserId,
                requestOrderHistoryDto.StartDate,
                requestOrderHistoryDto.EndDate);

            if (!orders.Any())
            {
                throw new KeyNotFoundException(OrderResources.NotFound);
            }

            var responseOrders = MapResponseOrderDto(orders);

            var responseOrderHistory = new ResponseOrderHistoryDto()
            {
                userDto = mapper.Map<UserDto>(await userRepository.GetByIdAsync(requestOrderHistoryDto.UserId))
            };

            foreach (var item in responseOrders.Records)
            {
                responseOrderHistory.TotalCost += item.Foods.Select(food => food.Price).Sum();
                var fitnessMenus = item.Foods.Count(food => food.Category == Category.Fitness);
                responseOrderHistory.FitnessMenusOrdered += fitnessMenus;
                responseOrderHistory.FullMenusOrdered += item.Foods.Count - fitnessMenus;
            }

            return responseOrderHistory;
        }

        public async Task<ResponseDto<ResponseOrderDto>> GetTodaysOrders()
        {
            var orders = await orderRepository.GetTodaysOrdersAsync();

            return MapResponseOrderDto(orders);
        }

        public async Task<ResponseOrderDto> GetTodaysOrderByUser(Guid userId)
        {
            jwtUtils.CheckForOwnership(httpContext, userId);

            var order = await orderRepository.GetTodaysOrderByUserAsync(userId);

            if (order is null)
            {
                throw new KeyNotFoundException(OrderResources.NotFound);
            }

            return MapResponseOrderDto(order);
        }

        public async Task<ResponseOrderDto> Update(UpdateOrderDto updatedOrderDto)
        {
            jwtUtils.CheckForOwnership(httpContext, updatedOrderDto.UserId);

            var order = await orderRepository.GetTodaysOrderAsync(updatedOrderDto.UserId);

            if (order is null)
            {
                throw new KeyNotFoundException(OrderResources.NotFound);
            }

            order = mapper.Map(updatedOrderDto, order);

            order.OrderFoods
                .ToList()
                .Clear();

            order.OrderFoods
                .ToList()
                .AddRange(updatedOrderDto.Foods
                .Select(uo => new OrderFood() { FoodId = uo, OrderId = order.Id }));

            order = await orderRepository.UpdateAsync(order);

            return await GetById(order.Id);
        }

        private ResponseDto<ResponseOrderDto> MapResponseOrderDto(IEnumerable<Order> orders)
        {
            var responseOrders = mapper.Map<ResponseDto<ResponseOrderDto>>(orders);

            for (var i = 0; i < responseOrders.Records.Count(); i++)
            {
                responseOrders.Records.ElementAt(i).Foods
                    .AddRange(orders
                    .First(o => o.Id == responseOrders.Records.ElementAt(i).Id).OrderFoods
                    .Select(of => mapper.Map<FoodDto>(of.Food)));
            }

            return responseOrders;
        }

        private ResponseOrderDto MapResponseOrderDto(Order order)
        {
            var responseOrder = mapper.Map<ResponseOrderDto>(order);

            responseOrder.Foods
                .AddRange(order.OrderFoods
                .Select(of => mapper.Map<FoodDto>(of.Food)));

            return responseOrder;
        }
    }
}
