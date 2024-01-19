using Assist.Lunch._4.Controllers;
using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.RestaurantDtos;
using Assist.Lunch._4.Core.Dtos.UserDtos;
using Assist.Lunch._4.Core.Helpers.ExceptionHandler.CustomExceptions;
using Assist.Lunch._4.Core.Helpers.Mapper.MappingProfiles;
using Assist.Lunch._4.Core.Services;
using Assist.Lunch._4.Domain.Entitites;
using Assist.Lunch._4.Infrastructure.Interfaces;
using Assist.Lunch._4.Resources;
using Assist.Lunch._4.Tests.Mocks.MockDtos;
using Assist.Lunch._4.Tests.Mocks.MockEntities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Assist.Lunch._4.Tests.Controllers
{
    public class RestaurantControllerTests
    {
        private readonly Mock<IRestaurantRepository> mockRestaurantRepository;
        private readonly IMapper mapper;
        private readonly RestaurantController restaurantController;
        private readonly RestaurantService restaurantService;
        private readonly IEnumerable<Restaurant> mockRestaurantList;

        public RestaurantControllerTests()
        {
            mockRestaurantRepository = new Mock<IRestaurantRepository>();

            mapper = new MapperConfiguration(cfg => cfg.AddProfile<RestaurantProfileMapper>()).CreateMapper();

            mockRestaurantList = MockRestaurantEntity.PopulateRestaurantList();

            restaurantService = new RestaurantService(
                mockRestaurantRepository.Object,
                mapper);

            restaurantController = new RestaurantController(restaurantService);
        }

        [Fact]
        public async Task Get_ReturnsOkActionResult_WhenDbTableIsPopulated()
        {
            // Arrange
            mockRestaurantRepository
                .Setup(restaurantRepository => restaurantRepository
                .GetAllAsync())
                .ReturnsAsync(mockRestaurantList);

            // Act
            var result = await restaurantController.Get() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<ResponseDto<ResponseRestaurantDto>>(result.Value);
            Assert.Equal(mockRestaurantList.Count(), ((ResponseDto<ResponseRestaurantDto>)result.Value).Records.Count());
        }

        [Fact]
        public async Task Get_ReturnOkActionResult_WhenProvidedValidGuid()
        {
            // Arrange
            mockRestaurantRepository
                .Setup(restaurantRepository => restaurantRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid id) => mockRestaurantList.FirstOrDefault(restaurant => restaurant.Id == id));

            // Act
            var result = await restaurantController.Get(mockRestaurantList.First().Id) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<ResponseRestaurantDto>(result.Value);
            Assert.Equal(mockRestaurantList.ElementAt(0).Id, result.Value.GetType().GetProperty("Id").GetValue(result.Value));
        }

        [Fact]
        public async Task Get_ThrowsNotFoundException_WhenProvidedInvalidGuid()
        {
            // Arrange
            mockRestaurantRepository
                .Setup(restaurantRepository => restaurantRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .Throws(new KeyNotFoundException(RestaurantResources.NotFound));

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await restaurantController.Get(new Guid()));

            //Assert
            Assert.IsType<KeyNotFoundException>(result);
        }

        [Fact]
        public async Task Add_ReturnsOkActionResult_WhenProvidedValidNewRestaurant()
        {
            // Arrange
            var mockBaseRestaurantDto = MockDtoEntities<BaseRestaurantDto>.PopulateEntity();

            mockRestaurantRepository
                .Setup(restaurantRepository => restaurantRepository
                .InsertAsync(It.IsAny<Restaurant>()))
                .ReturnsAsync(mockRestaurantList.First());

            mockRestaurantRepository
                .Setup(restaurantRepository => restaurantRepository
                .GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await restaurantController.Add(mockBaseRestaurantDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<ResponseRestaurantDto>(result.Value);
            Assert.Equal(mockRestaurantList.ElementAt(0).Id, result.Value.GetType().GetProperty("Id").GetValue(result.Value));
        }

        [Fact]
        public async Task Add_ThrowsEntityAlreadyExistsException_WhenProvidedExistingRestaurant()
        {
            // Arrange
            var mockBaseRestaurantDto = MockDtoEntities<BaseRestaurantDto>.PopulateEntity();

            mockRestaurantRepository
                .Setup(restaurantRepository => restaurantRepository
                .GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(mockRestaurantList.ElementAt(0));

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await restaurantController.Add(mockBaseRestaurantDto));

            //Assert
            Assert.IsType<EntityAlreadyExistsException>(result);
        }

        [Fact]
        public async Task Update_ReturnOkActionResult_WhenProvidedValidRestaurantDto()
        {
            // Arrange
            var mockRestaurantDto = MockDtoEntities<RestaurantDto>.PopulateEntity();

            mockRestaurantRepository
                .Setup(restaurantRepository => restaurantRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(mockRestaurantList.ElementAt(0));

            mockRestaurantRepository
                .Setup(restaurantRepository => restaurantRepository
                .UpdateAsync(It.IsAny<Restaurant>()))
                .ReturnsAsync(mockRestaurantList.ElementAt(0));

            // Act
            var result = await restaurantController.Update(mockRestaurantDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<ResponseRestaurantDto>(result.Value);
            Assert.Equal(mockRestaurantList.ElementAt(0).Id, result.Value.GetType().GetProperty("Id").GetValue(result.Value));
        }

        [Fact]
        public async Task Update_ThrowsNotFoundException_WhenProvidedInvalidRestaurantDto()
        {
            // Arrange
            var mockRestaurantDto = MockDtoEntities<RestaurantDto>.PopulateEntity();

            mockRestaurantRepository
                .Setup(restaurantRepository => restaurantRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            mockRestaurantRepository
                .Setup(restaurantRepository => restaurantRepository
                .UpdateAsync(It.IsAny<Restaurant>()))
                .ReturnsAsync(mockRestaurantList.ElementAt(0));

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await restaurantController.Update(mockRestaurantDto));

            //Assert
            Assert.IsType<KeyNotFoundException>(result);
        }

        [Fact]
        public async Task Delete_OkObjectResult_WhenProvidedValidGuid()
        {
            // Arrange
            mockRestaurantRepository
                .Setup(restaurantRepository => restaurantRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(mockRestaurantList.ElementAt(0));

            mockRestaurantRepository
                .Setup(restaurantRepository => restaurantRepository
                .DeleteAsync(It.IsAny<Guid>()));

            // Act
            var result = await restaurantController.Delete(mockRestaurantList.ElementAt(0).Id);

            // Assert
            Assert.Equal(200, result.GetType().GetProperty("StatusCode").GetValue(result));
        }

        [Fact]
        public async Task Delete_ThrowsNotFoundException_WhenProvidedInvalidGuid()
        {
            // Arrange
            mockRestaurantRepository
               .Setup(restaurantRepository => restaurantRepository
               .GetByIdAsync(It.IsAny<Guid>()))
               .ReturnsAsync(() => null);

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await restaurantController.Delete(new Guid()));

            //Assert
            Assert.IsType<KeyNotFoundException>(result);
        }
    }
}
