using Assist.Lunch._4.Controllers;
using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.FoodDtos;
using Assist.Lunch._4.Core.Dtos.UserDtos;
using Assist.Lunch._4.Core.Helpers.ExceptionHandler.CustomExceptions;
using Assist.Lunch._4.Core.Helpers.Mapper.MappingProfiles;
using Assist.Lunch._4.Core.Services;
using Assist.Lunch._4.Domain.Entitites;
using Assist.Lunch._4.Infrastructure.Interfaces;
using Assist.Lunch._4.Tests.Mocks.MockDtos;
using Assist.Lunch._4.Tests.Mocks.MockEntities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Assist.Lunch._4.Tests.Controllers
{
    public class FoodControllerTests
    {
        private readonly Mock<IFoodRepository> mockFoodRepository;
        private readonly Mock<IRestaurantRepository> mockRestaurantRepository;
        private readonly IMapper mapper;
        private readonly FoodController foodController;
        private readonly FoodService foodService;
        private readonly IEnumerable<Food> mockFoodList;

        public FoodControllerTests()
        {
            mockFoodRepository = new Mock<IFoodRepository>();
            mockRestaurantRepository = new Mock<IRestaurantRepository>();

            mapper = new MapperConfiguration(cfg => cfg.AddProfile<FoodProfileMapper>()).CreateMapper();

            mockFoodList = MockFoodEntity.PopulateFoodList();

            foodService = new FoodService(
                mockFoodRepository.Object,
                mockRestaurantRepository.Object,
                mapper);

            foodController = new FoodController(foodService);
        }

        [Fact]
        public async Task Get_ReturnsOkActionResult_WhenDbTableIsPopulated()
        {
            // Arrange
            mockFoodRepository
                .Setup(foodRepository => foodRepository
                .GetAllAsync())
                .ReturnsAsync(mockFoodList);

            // Act
            var result = await foodController.Get() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<ResponseDto<ResponseFoodDto>>(result.Value);
            Assert.Equal(mockFoodList.Count(), ((ResponseDto<ResponseFoodDto>)result.Value).Records.Count());
        }

        [Fact]
        public async Task Get_ReturnOkActionResult_WhenProvidedValidGuid()
        {
            // Arrange
            mockFoodRepository
                .Setup(foodRepository => foodRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid id) => mockFoodList.FirstOrDefault(food => food.Id == id));

            // Act
            var result = await foodController.Get(mockFoodList.ElementAt(0).Id) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<ResponseFoodDto>(result.Value);
            Assert.Equal(mockFoodList.ElementAt(0).Id, result.Value.GetType().GetProperty("Id").GetValue(result.Value));
        }

        [Fact]
        public async Task Get_ThrowsNotFoundException_WhenProvidedInvalidGuid()
        {
            // Arrange
            mockFoodRepository
                .Setup(foodRepository => foodRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await foodController.Get(new Guid()));

            //Assert
            Assert.IsType<KeyNotFoundException>(result);
        }

        [Fact]
        public async Task Add_ReturnsOkActionResult_ProvidedValidNewFood()
        {
            // Arrange
            var mockAddFoodDto = MockDtoEntities<AddFoodDto>.PopulateEntity();
            var mockRestaurant = MockRestaurantEntity.PopulateRestaurant();

            mockRestaurantRepository
                .Setup(foodRepository => foodRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(mockRestaurant);

            mockFoodRepository
                .Setup(foodRepository => foodRepository
                .GetByRestaurantAndNameAsync(mockRestaurant.Id, mockAddFoodDto.Name))
                .ReturnsAsync(() => null);

            mockFoodRepository
                .Setup(foodRepository => foodRepository
                .InsertAsync(It.IsAny<Food>()))
                .ReturnsAsync(mockFoodList.ElementAt(0));

            // Act
            var result = await foodController.Add(mockAddFoodDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<ResponseFoodDto>(result.Value);
            Assert.Equal(mockFoodList.ElementAt(0).Id, result.Value.GetType().GetProperty("Id").GetValue(result.Value));
        }

        [Fact]
        public async Task Add_ThrowsEntityAlreadyExistsException_WhenProvidedExistingFood()
        {
            // Arrange
            var mockAddFoodDto = MockDtoEntities<AddFoodDto>.PopulateEntity();
            var mockRestaurant = MockRestaurantEntity.PopulateRestaurant();

            mockRestaurantRepository
                .Setup(foodRepository => foodRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(mockRestaurant);

            mockFoodRepository
                .Setup(foodRepository => foodRepository
                .GetByRestaurantAndNameAsync(It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(mockFoodList.ElementAt(0));

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await foodController.Add(mockAddFoodDto));

            //Assert
            Assert.IsType<EntityAlreadyExistsException>(result);
        }

        [Fact]
        public async Task Add_ThrowsKeyNotFoundException_WhenProvidedExistingFood()
        {
            // Arrange
            var mockAddFoodDto = MockDtoEntities<AddFoodDto>.PopulateEntity();
            var mockRestaurant = MockRestaurantEntity.PopulateRestaurant();

            mockRestaurantRepository
                .Setup(foodRepository => foodRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await foodController.Add(mockAddFoodDto));

            //Assert
            Assert.IsType<KeyNotFoundException>(result);
        }

        [Fact]
        public async Task Update_ReturnOkActionResult_WhenProvidedValidFoodDto()
        {
            // Arrange
            var mockUpdateFoodDto = MockDtoEntities<UpdateFoodDto>.PopulateEntity();

            mockFoodRepository
                .Setup(foodRepository => foodRepository
                .UpdateAsync(mockFoodList.ElementAt(0)))
                .ReturnsAsync(mockFoodList.First());

            mockFoodRepository
                .Setup(foodRepository => foodRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(mockFoodList.ElementAt(0));

            // Act
            var result = await foodController.Update(mockUpdateFoodDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<ResponseFoodDto>(result.Value);
            Assert.Equal(mockFoodList.ElementAt(0).Id, result.Value.GetType().GetProperty("Id").GetValue(result.Value));
        }

        [Fact]
        public async Task Update_ThrowsNotFoundException_WhenProvidedInvalidFoodDto()
        {
            // Arrange
            var mockUpdateFoodDto = MockDtoEntities<UpdateFoodDto>.PopulateEntity();

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await foodController.Update(mockUpdateFoodDto));

            // Assert
            Assert.IsType<KeyNotFoundException>(result);
        }

        [Fact]
        public async Task Delete_OkObjectResult_WhenProvidedValidGuid()
        {
            // Arrange
            mockFoodRepository
                .Setup(foodService => foodService
                .DeleteAsync(It.IsAny<Guid>()));

            mockFoodRepository
                .Setup(foodRepository => foodRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(mockFoodList.ElementAt(0));

            // Act
            var result = await foodController.Delete(new Guid());

            // Assert
            Assert.Equal(200, result.GetType().GetProperty("StatusCode").GetValue(result));
        }

        [Fact]
        public async Task Delete_ThrowsNotFoundException_WhenProvidedInvalidGuid()
        {
            // Arrange
            mockFoodRepository
                .Setup(foodRepository => foodRepository
                .DeleteAsync(It.IsAny<Guid>()));

            mockFoodRepository
                .Setup(foodRepository => foodRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await foodController.Delete(new Guid()));

            //Assert
            Assert.IsType<KeyNotFoundException>(result);
        }

        [Fact]
        public async Task GetByRestaurant_ReturnOkActionResult_WhenProvidedValidRestaurantGuid()
        {
            // Arrange
            mockFoodRepository.Setup(foodRepository => foodRepository.GetByRestaurantAsync(It.IsAny<Guid>()))
                .ReturnsAsync(mockFoodList);

            // Act
            var result = await foodController.GetByRestaurant(new Guid()) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<ResponseDto<ResponseFoodDto>>(result.Value);
            Assert.Equal(mockFoodList.Count(), ((ResponseDto<ResponseFoodDto>)result.Value).Records.Count());
        }

        [Fact]
        public async Task GetByRestaurant_ThrowsNotFoundException_WhenProvidedInvalidGuid()
        {
            // Arrange
            mockFoodRepository.Setup(foodRepository => foodRepository.GetByRestaurantAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new List<Food>());

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await foodController.GetByRestaurant(new Guid()));

            //Assert
            Assert.IsType<KeyNotFoundException>(result);
        }
    }
}
