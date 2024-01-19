using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.RestaurantDtos;
using Assist.Lunch._4.Core.Helpers.ExceptionHandler.CustomExceptions;
using Assist.Lunch._4.Core.Helpers.Mapper.MappingProfiles;
using Assist.Lunch._4.Core.Services;
using Assist.Lunch._4.Domain.Entitites;
using Assist.Lunch._4.Infrastructure.Interfaces;
using Assist.Lunch._4.Resources;
using Assist.Lunch._4.Tests.Mocks.MockDtos;
using Assist.Lunch._4.Tests.Mocks.MockEntities;
using AutoMapper;
using Moq;

namespace Assist.Lunch._4.Tests.Services
{
    public class RestaurantServiceTests
    {
        private readonly Mock<IRestaurantRepository> mockRestaurantRepository;
        private readonly IMapper mapper;
        private readonly RestaurantService restaurantService;
        private readonly IEnumerable<Restaurant> restaurantList;

        public RestaurantServiceTests()
        {
            mockRestaurantRepository = new Mock<IRestaurantRepository>();

            restaurantList = MockRestaurantEntity.PopulateRestaurantList();

            mapper = new MapperConfiguration(cfg => cfg.AddProfile<RestaurantProfileMapper>()).CreateMapper();

            restaurantService = new RestaurantService(
                mockRestaurantRepository.Object,
                mapper);
        }

        [Fact]
        public async Task GetAll_ReturnsRestaurantsList_WhenDbTableIsPopulated()
        {
            // Arrange
            mockRestaurantRepository
                .Setup(mockRestaurantRepository => mockRestaurantRepository
                .GetAllAsync())
                .ReturnsAsync(restaurantList);

            // Act
            var result = restaurantService.GetAll();

            // Assert
            await Assert.IsType<Task<ResponseDto<ResponseRestaurantDto>>>(result);
            var restaurants = await mockRestaurantRepository.Object.GetAllAsync();
            Assert.Equal(restaurantList.Count(), restaurants.Count());
        }

        [Fact]
        public async Task GetById_ReturnsRestaurant_WhenProvidedValidGuid()
        {
            // Arrange
            mockRestaurantRepository
                .Setup(mockRestaurantRepository => mockRestaurantRepository
                .GetByIdAsync(restaurantList.First().Id))
                .ReturnsAsync(restaurantList.First());

            // Act
            var result = restaurantService.GetById(restaurantList.First().Id);

            // Assert
            await Assert.IsType<Task<ResponseRestaurantDto>>(result);
        }

        [Fact]
        public async Task GetById_ThrowsNotFound_WhenProvidedInvalidGuid()
        {
            // Assert
            var ex = await Assert
                .ThrowsAsync<KeyNotFoundException>(() =>
                restaurantService.GetById(new Guid()));
            Assert.Equal(ex.Message, RestaurantResources.NotFound);
        }

        [Fact]
        public async Task Add_ReturnResponseRestaurantDto_WhenProvidedValidNewRestaurant()
        {
            // Arrange 
            var mockBaseRestaurantDto = MockDtoEntities<BaseRestaurantDto>.PopulateEntity();

            mockRestaurantRepository
                .Setup(mockRestaurantRepository => mockRestaurantRepository
                .GetByNameAsync(mockBaseRestaurantDto.Name))
                .ReturnsAsync(() => null);

            // Act
            var result = restaurantService.Add(mockBaseRestaurantDto);

            // Assert
            await Assert.IsType<Task<ResponseRestaurantDto>>(result);
        }

        [Fact]
        public async Task Add_ThrowsRestaurantAlreadyAdded_WhenProvidedExistingRestaurant()
        {
            // Arrange 
            var mockBaseRestaurantDto = MockDtoEntities<BaseRestaurantDto>.PopulateEntity();

            mockRestaurantRepository
                .Setup(mockRestaurantRepository => mockRestaurantRepository
                .GetByNameAsync(mockBaseRestaurantDto.Name))
                .ReturnsAsync(restaurantList.First());

            // Act
            var result = restaurantService.Add(mockBaseRestaurantDto);

            // Assert
            var ex = await Assert
                .ThrowsAsync<EntityAlreadyExistsException>(() =>
                restaurantService.Add(mockBaseRestaurantDto));
            Assert.Equal(ex.Message, RestaurantResources.AlreadyExists);
        }

        [Fact]
        public async Task Update_ReturnResponseRestaurantDto_WhenProvidedValidRestaurant()
        {
            // Arrange 
            var mockRestaurantDto = MockDtoEntities<RestaurantDto>.PopulateEntity();

            mockRestaurantRepository
                .Setup(mockRestaurantRepository => mockRestaurantRepository
                .GetByIdAsync(mockRestaurantDto.Id))
                .ReturnsAsync(restaurantList.First());

            // Act
            var result = restaurantService.Update(mockRestaurantDto);

            // Assert
            await Assert.IsType<Task<ResponseRestaurantDto>>(result);
        }

        [Fact]
        public async Task Update_ThrowsNotFound_WhenProvidedInvalidRestaurant()
        {
            // Arrange 
            var mockRestaurantDto = MockDtoEntities<RestaurantDto>.PopulateEntity();

            mockRestaurantRepository
                .Setup(mockRestaurantRepository => mockRestaurantRepository
                .GetByIdAsync(mockRestaurantDto.Id))
                .ReturnsAsync(() => null);

            // Act
            var result = restaurantService.Update(mockRestaurantDto);

            // Assert
            var ex = await Assert
                .ThrowsAsync<KeyNotFoundException>(() =>
                restaurantService.Update(mockRestaurantDto));
            Assert.Equal(ex.Message, RestaurantResources.NotFound);
        }

        [Fact]
        public async Task Delete_ChangeProperty_WhenProvidedValidRestaurant()
        {
            // Arrange
            mockRestaurantRepository
                .Setup(mockRestaurantRepository => mockRestaurantRepository
                .GetByIdAsync(restaurantList.First().Id))
                .ReturnsAsync(restaurantList.First());

            // Act
            var response = await restaurantService.Delete(restaurantList.First().Id);

            // Assert
            Assert.True(restaurantList.First().IsDeleted);
        }

        [Fact]
        public async Task Delete_ThrowsNotFound_WhenProvidedInvalidRestaurant()
        {
            // Arrange
            mockRestaurantRepository
                .Setup(mockRestaurantRepository => mockRestaurantRepository
                .GetByIdAsync(restaurantList.First().Id))
                .ReturnsAsync(() => null);

            // Act
            var response = restaurantService.Delete(restaurantList.First().Id);

            // Assert
            var ex = await Assert
                .ThrowsAsync<KeyNotFoundException>(() =>
                restaurantService.Delete(restaurantList.First().Id));
            Assert.Equal(ex.Message, RestaurantResources.NotFound);
        }
    }
}
