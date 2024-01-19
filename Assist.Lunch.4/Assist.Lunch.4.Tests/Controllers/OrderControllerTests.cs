using Assist.Lunch._4.Controllers;
using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.OrderDtos;
using Assist.Lunch._4.Core.Dtos.UserDtos;
using Assist.Lunch._4.Core.Helpers.ExceptionHandler.CustomExceptions;
using Assist.Lunch._4.Core.Helpers.Mapper.MappingProfiles;
using Assist.Lunch._4.Core.Security.Utils;
using Assist.Lunch._4.Core.Services;
using Assist.Lunch._4.Domain.Entitites;
using Assist.Lunch._4.Infrastructure.Interfaces;
using Assist.Lunch._4.Tests.Mocks.MockDtos;
using Assist.Lunch._4.Tests.Mocks.MockEntities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Moq;

namespace Assist.Lunch._4.Tests.Controllers
{
    public class OrderControllerTests
    {
        private readonly Mock<IUserRepository> mockUserRepository;
        private readonly Mock<IOrderRepository> mockOrderRepository;
        private readonly Mock<IFoodRepository> mockFoodRepository;
        private readonly Mock<IHttpContextAccessor> mockHttpContext;
        private readonly Mock<IJwtUtils> mockJwtUtils;
        private readonly IMapper mapper;
        private readonly OrderController orderController;
        private readonly OrderService orderService;
        private readonly IEnumerable<Order> mockOrderList;
        private readonly IEnumerable<Food> mockFoodList;

        public OrderControllerTests()
        {
            mockUserRepository = new Mock<IUserRepository>();
            mockOrderRepository = new Mock<IOrderRepository>();
            mockFoodRepository = new Mock<IFoodRepository>();
            mockHttpContext = new Mock<IHttpContextAccessor>();
            mockJwtUtils = new Mock<IJwtUtils>();

            mapper = new MapperConfiguration(cfg => cfg.AddProfile<OrderProfileMapper>()).CreateMapper();

            mockOrderList = MockOrderEntity.PopulateOrderList();
            mockFoodList = MockFoodEntity.PopulateFoodList();

            orderService = new OrderService(
                mockOrderRepository.Object,
                mockUserRepository.Object,
                mapper,
                mockFoodRepository.Object,
                mockHttpContext.Object,
                mockJwtUtils.Object);

            orderController = new OrderController(orderService);
        }

        [Fact]
        public async Task Get_ReturnsOkActionResult_WhenDbTableIsPopulated()
        {
            // Arrange
            mockOrderRepository
                .Setup(orderRepository => orderRepository
                .GetAllActiveAsync())
                .ReturnsAsync(mockOrderList);

            // Act
            var result = await orderController.Get() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<ResponseDto<ResponseOrderDto>>(result.Value);
            Assert.Equal(mockOrderList.Count(), ((ResponseDto<ResponseOrderDto>)result.Value).Records.Count());
        }

        [Fact]
        public async Task GetByUser_ReturnsOkActionResult_WhenProvidedValidUserGuid()
        {
            // Arrange
            mockOrderRepository
                .Setup(orderRepository => orderRepository
                .GetOrdersByUserAsync(It.IsAny<Guid>(), null, null))
                .ReturnsAsync(mockOrderList);

            // Act
            var result = await orderController.Get(new Guid()) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<ResponseDto<ResponseOrderDto>>(result.Value);
            Assert.Equal(mockOrderList.Count(), ((ResponseDto<ResponseOrderDto>)result.Value).Records.Count());
        }

        [Fact]
        public async Task GetByUser_ThrowsNotFoundException_WhenDbOrderTableIsEmpty()
        {
            // Arrange
            mockOrderRepository
                .Setup(orderRepository => orderRepository
                .GetOrdersByUserAsync(It.IsAny<Guid>(), null, null))
                .ReturnsAsync(new List<Order>());

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await orderController.Get(new Guid()));

            //Assert
            Assert.IsType<KeyNotFoundException>(result);
        }

        [Fact]
        public async Task GetByOrder_ReturnsOkActionResult_WhenProvidedValidOrderGuid()
        {
            // Arrange
            mockOrderRepository
                .Setup(orderRepository => orderRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(mockOrderList.First());

            // Act
            var result = await orderController.GetBy(new Guid()) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<ResponseOrderDto>(result.Value);
            Assert.Equal(mockOrderList.First().Id, result.Value.GetType().GetProperty("Id").GetValue(result.Value));
        }

        [Fact]
        public async Task GetByOrder_ThrowsNotFoundException_WhenDbOrderTableIsEmpty()
        {
            // Arrange
            mockOrderRepository
                .Setup(orderRepository => orderRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await orderController.GetBy(new Guid()));

            //Assert
            Assert.IsType<KeyNotFoundException>(result);
        }

        [Fact]
        public async Task Add_ReturnsOkActionResult_WhenProvidedValidNewOrder()
        {
            // Arrange
            var mockBaseOrderDto = MockDtoEntities<BaseOrderDto>.PopulateEntity();

            mockHttpContext
                .Setup(o => o
                .HttpContext.Request.Headers[HeaderNames.Authorization]);

            mockJwtUtils
                .Setup(jwt => jwt
                .CheckForOwnership(It.IsAny<IHttpContextAccessor>(), It.IsAny<Guid>()));

            mockOrderRepository
                .Setup(orderRepository => orderRepository
                .GetTodaysOrdersAsync())
                .ReturnsAsync(mockOrderList);

            mockOrderRepository
                .Setup(orderRepository => orderRepository
                .InsertAsync(It.IsAny<Order>()))
                .ReturnsAsync(mockOrderList.First());

            mockFoodRepository
                .Setup(mockFoodRepository => mockFoodRepository
                .GetAllAsync())
                .ReturnsAsync(mockFoodList);

            mockOrderRepository
                .Setup(orderRepository => orderRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(mockOrderList.First());

            // Act
            var result = await orderController.Add(mockBaseOrderDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<ResponseOrderDto>(result.Value);
            Assert.Equal(mockOrderList.First().Id, result.Value.GetType().GetProperty("Id").GetValue(result.Value));
        }

        [Fact]
        public async Task Add_ThrowsEntityAlreadyExistsException_WhenProvidedExistingOrder()
        {
            // Arrange
            var mockBaseOrderDto = MockDtoEntities<BaseOrderDto>.PopulateEntity();

            mockHttpContext
                .Setup(o => o
                .HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(It.IsAny<string>());

            mockJwtUtils
                .Setup(jwt => jwt
                .CheckForOwnership(It.IsAny<IHttpContextAccessor>(), It.IsAny<Guid>()));

            mockOrderRepository
                .Setup(orderRepository => orderRepository
                .GetTodaysOrderByUserAsync(It.IsAny<Guid>()))
                .ReturnsAsync(mockOrderList.First());

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await orderController.Add(mockBaseOrderDto));

            //Assert
            Assert.IsType<EntityAlreadyExistsException>(result);
        }

        [Fact]
        public async Task Add_ThrowsInvalidCredentialsException_WhenUserIsNorAdminOrOrderOwner()
        {
            // Arrange
            var mockBaseOrderDto = MockDtoEntities<BaseOrderDto>.PopulateEntity();

            mockHttpContext
                .Setup(o => o
                .HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(It.IsAny<string>());

            mockJwtUtils
                .Setup(jwt => jwt
                .CheckForOwnership(It.IsAny<IHttpContextAccessor>(), It.IsAny<Guid>()))
                .Throws(new InvalidCredentialsException());

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await orderController.Add(mockBaseOrderDto));

            //Assert
            Assert.IsType<InvalidCredentialsException>(result);
        }

        [Fact]
        public async Task Update_ReturnOkActionResult_WhenProvidedValidOrder()
        {
            // Arrange
            var mockOrderDto = MockDtoEntities<UpdateOrderDto>.PopulateEntity();

            mockHttpContext
                .Setup(o => o
                .HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(It.IsAny<string>());

            mockJwtUtils
                .Setup(jwt => jwt
                .CheckForOwnership(It.IsAny<IHttpContextAccessor>(), It.IsAny<Guid>()));

            mockOrderRepository
                .Setup(orderRepository => orderRepository
                .GetTodaysOrderAsync(It.IsAny<Guid>()))
                .ReturnsAsync(mockOrderList.First());

            mockOrderRepository
                .Setup(orderRepository => orderRepository
                .InsertAsync(It.IsAny<Order>()))
                .ReturnsAsync(mockOrderList.First());

            mockOrderRepository
                .Setup(mockFoodRepository => mockFoodRepository
                .UpdateAsync(It.IsAny<Order>()))
                .ReturnsAsync(mockOrderList.First());

            mockOrderRepository
                .Setup(orderRepository => orderRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(mockOrderList.First());

            // Act
            var result = await orderController.Update(mockOrderDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<ResponseOrderDto>(result.Value);
            Assert.Equal(mockOrderList.First().Id, result.Value.GetType().GetProperty("Id").GetValue(result.Value));
        }

        [Fact]
        public async Task Update_ThrowsNotFoundException_WhenProvidedInvalidOrderDto()
        {
            // Arrange
            var mockOrderDto = MockDtoEntities<UpdateOrderDto>.PopulateEntity();

            mockHttpContext
                .Setup(o => o
                .HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(It.IsAny<string>());

            mockJwtUtils
                .Setup(jwt => jwt
                .CheckForOwnership(It.IsAny<IHttpContextAccessor>(), It.IsAny<Guid>()));

            mockOrderRepository
                .Setup(orderRepository => orderRepository
                .GetTodaysOrderAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            mockOrderRepository
                .Setup(orderRepository => orderRepository
                .GetTodaysOrderByUserAsync(It.IsAny<Guid>()))
                .ReturnsAsync(mockOrderList.First());

            mockOrderRepository
                .Setup(orderRepository => orderRepository
                .DeleteAsync(It.IsAny<Guid>()));

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await orderController.Update(mockOrderDto));

            //Assert
            Assert.IsType<KeyNotFoundException>(result);
        }

        [Fact]
        public async Task Delete_OkObjectResult_WhenProvidedValidGuid()
        {
            // Arrange
            mockHttpContext
                .Setup(o => o
                .HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(It.IsAny<string>());

            mockJwtUtils
                .Setup(jwt => jwt
                .CheckForOwnership(It.IsAny<IHttpContextAccessor>(), It.IsAny<Guid>()));

            mockOrderRepository
                .Setup(orderRepository => orderRepository
                .GetTodaysOrderByUserAsync(It.IsAny<Guid>()))
                .ReturnsAsync(mockOrderList.First());

            // Act
            var result = await orderController.Delete(new Guid());

            // Assert
            Assert.Equal(200, result.GetType().GetProperty("StatusCode").GetValue(result));
        }

        [Fact]
        public async Task Delete_ThrowsNotFoundException_WhenProvidedInvalidGuid()
        {
            // Arrange
            mockHttpContext
                .Setup(o => o
                .HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(It.IsAny<string>());

            mockJwtUtils
                .Setup(jwt => jwt
                .CheckForOwnership(It.IsAny<IHttpContextAccessor>(), It.IsAny<Guid>()));

            mockOrderRepository
                .Setup(orderRepository => orderRepository
                .GetTodaysOrderByUserAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await orderController.Delete(new Guid()));

            //Assert
            Assert.IsType<KeyNotFoundException>(result);
        }

        [Fact]
        public async Task Delete_ThrowsInvalidCredentialsException_WhenUserIsNorAdminOrOrderOwner()
        {
            // Arrange
            mockHttpContext
                .Setup(o => o
                .HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(It.IsAny<string>());

            mockJwtUtils
                .Setup(jwt => jwt
                .CheckForOwnership(It.IsAny<IHttpContextAccessor>(), It.IsAny<Guid>()));

            mockOrderRepository
                .Setup(orderRepository => orderRepository
                .GetTodaysOrderByUserAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await orderController.Delete(new Guid()));

            //Assert
            Assert.IsType<KeyNotFoundException>(result);
        }

        [Fact]
        public async Task GetOrderHistory_ReturnOkActionResult_WhenProvidedValidRequestOrderHistoryDto()
        {
            // Arrange
            var mockRequestOrderHistoryDto = MockDtoEntities<RequestOrderHistoryDto>.PopulateEntity();
            var mockResponseOrderHistoryDto = MockDtoEntities<ResponseOrderHistoryDto>.PopulateEntity();

            mockHttpContext
                .Setup(o => o
                .HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(It.IsAny<string>());

            mockJwtUtils
                .Setup(jwt => jwt
                .CheckForOwnership(It.IsAny<IHttpContextAccessor>(), It.IsAny<Guid>()));

            mockOrderRepository
                .Setup(orderRepository => orderRepository
                .GetOrdersByUserAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(mockOrderList);

            // Act
            var result = await orderController.GetOrderHistory(mockRequestOrderHistoryDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<ResponseOrderHistoryDto>(result.Value);
        }

        [Fact]
        public async Task GetOrderHistory_ThrowsNotFoundException_WhenProvidedInvalidRequestOrderHistoryDto()
        {
            // Arrange
            var mockRequestOrderHistoryDto = MockDtoEntities<RequestOrderHistoryDto>.PopulateEntity();

            mockHttpContext
                .Setup(o => o
                .HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(It.IsAny<string>());

            mockJwtUtils
                .Setup(jwt => jwt
                .CheckForOwnership(It.IsAny<IHttpContextAccessor>(), It.IsAny<Guid>()));

            mockOrderRepository
                .Setup(orderRepository => orderRepository
                .GetOrdersByUserAsync(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .ReturnsAsync(new List<Order>());

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await orderController.GetOrderHistory(mockRequestOrderHistoryDto));

            //Assert
            Assert.IsType<KeyNotFoundException>(result);
        }

        [Fact]
        public async Task GetTodaysOrders_ReturnsOkActionResult_WhenDbTableHasTodaysOrders()
        {
            // Arrange
            mockHttpContext
                .Setup(o => o
                .HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(It.IsAny<string>());

            mockJwtUtils
                .Setup(jwt => jwt
                .CheckForOwnership(It.IsAny<IHttpContextAccessor>(), It.IsAny<Guid>()));

            mockOrderRepository
                .Setup(orderRepository => orderRepository
                .GetTodaysOrdersAsync())
                .ReturnsAsync(mockOrderList);

            // Act
            var result = await orderController.GetTodaysOrders() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<ResponseDto<ResponseOrderDto>>(result.Value);
            Assert.Equal(mockOrderList.Count(), ((ResponseDto<ResponseOrderDto>)result.Value).Records.Count());
        }

        [Fact]
        public async Task GetTodaysOrder_ReturnsOkActionResult_WhenUserHasPlacedAnOrder()
        {
            // Arrange
            mockHttpContext
                .Setup(o => o
                .HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(It.IsAny<string>());

            mockJwtUtils
                .Setup(jwt => jwt
                .CheckForOwnership(It.IsAny<IHttpContextAccessor>(), It.IsAny<Guid>()));

            mockOrderRepository
                .Setup(orderRepository => orderRepository
                .GetTodaysOrderByUserAsync(It.IsAny<Guid>()))
                .ReturnsAsync(mockOrderList.First());

            // Act
            var result = await orderController.GetTodaysOrder(new Guid()) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<ResponseOrderDto>(result.Value);
            Assert.Equal(mockOrderList.First().Id, result.Value.GetType().GetProperty("Id").GetValue(result.Value));
        }

        [Fact]
        public async Task GetTodaysOrder_ThrowsNotFoundException_WhenDbOrderTableIsEmpty()
        {
            // Arrange
            mockHttpContext
                .Setup(o => o
                .HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(It.IsAny<string>());

            mockJwtUtils
                .Setup(jwt => jwt
                .CheckForOwnership(It.IsAny<IHttpContextAccessor>(), It.IsAny<Guid>()));

            mockOrderRepository
                .Setup(orderRepository => orderRepository
                .GetTodaysOrderByUserAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await orderController.GetTodaysOrder(new Guid()));

            //Assert
            Assert.IsType<KeyNotFoundException>(result);
        }
    }
}
