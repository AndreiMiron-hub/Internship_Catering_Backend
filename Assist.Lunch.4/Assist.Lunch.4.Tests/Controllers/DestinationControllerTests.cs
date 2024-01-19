using Assist.Lunch._4.Controllers;
using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.DestinationDtos;
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
    public class DestinationControllerTests
    {
        private readonly Mock<IDestinationRepository> mockDestinationRepository;
        private readonly IMapper mapper;
        private readonly DestinationController destinationController;
        private readonly DestinationService destinationService;
        private readonly IEnumerable<Destination> mockDestinationList;

        public DestinationControllerTests()
        {
            mockDestinationRepository = new Mock<IDestinationRepository>();

            mapper = new MapperConfiguration(cfg => cfg.AddProfile<DestinationProfileMapper>()).CreateMapper();

            mockDestinationList = MockDestinationEntity.PopulateDestinationList();

            destinationService = new DestinationService(
                mockDestinationRepository.Object,
                mapper);

            destinationController = new DestinationController(destinationService);
        }

        [Fact]
        public async Task Get_ReturnsOkActionResult_WhenDbTableIsPopulated()
        {
            // Arrange
            mockDestinationRepository
                .Setup(destinationRepository => destinationRepository
                .GetAllAsync())
                .ReturnsAsync(mockDestinationList.AsEnumerable());

            // Act
            var result = await destinationController.Get() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<ResponseDto<ResponseDestinationDto>>(result.Value);
            Assert.Equal(mockDestinationList.Count(), ((ResponseDto<ResponseDestinationDto>)result.Value).Records.Count());
        }

        [Fact]
        public async Task Get_ReturnOkActionResult_WhenProvidedValidGuid()
        {
            // Arrange
            mockDestinationRepository
                .Setup(destinationRepository => destinationRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid id) => mockDestinationList.FirstOrDefault(destination => destination.Id == id));

            // Act
            var result = await destinationController.Get(mockDestinationList.ElementAt(0).Id) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<ResponseDestinationDto>(result.Value);
            Assert.Equal(mockDestinationList.ElementAt(0).Id, result.Value.GetType().GetProperty("Id").GetValue(result.Value));
        }

        [Fact]
        public async Task Get_ThrowsNotFoundException_WhenProvidedInvalidGuid()
        {
            // Arrange
            mockDestinationRepository
                .Setup(destinationRepository => destinationRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await destinationController.Get(new Guid()));

            // Assert
            Assert.IsType<KeyNotFoundException>(result);
        }

        [Fact]
        public async Task Add_ReturnsOkActionResult_WhenProvidedNewValidDestination()
        {
            // Arrange
            var mockBaseDestinationDto = MockDtoEntities<BaseDestinationDto>.PopulateEntity();

            mockDestinationRepository
                .Setup(destinationRepository => destinationRepository
                .InsertAsync(It.IsAny<Destination>()))
                .ReturnsAsync(mockDestinationList.ElementAt(0));

            // Act
            var result = await destinationController.Add(mockBaseDestinationDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<ResponseDestinationDto>(result.Value);
            Assert.Equal(mockDestinationList.ElementAt(0).Id, result.Value.GetType().GetProperty("Id").GetValue(result.Value));
        }

        [Fact]
        public async Task Add_ThrowsEntityAlreadyExistsException_WhenProvidedExistingDestination()
        {
            // Arrange
            var mockBaseDestinationDto = MockDtoEntities<BaseDestinationDto>.PopulateEntity();

            mockDestinationRepository
                .Setup(destinationRepository => destinationRepository
                .GetByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(mockDestinationList.ElementAt(0));

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await destinationController.Add(mockBaseDestinationDto));

            // Assert
            Assert.IsType<EntityAlreadyExistsException>(result);
        }

        [Fact]
        public async Task Update_ReturnOkActionResult_WhenProvidedValidDestinationDto()
        {
            // Arrange
            var mockDestinationDto = MockDtoEntities<DestinationDto>.PopulateEntity();

            mockDestinationRepository
                .Setup(destinationRepository => destinationRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(mockDestinationList.ElementAt(0));

            mockDestinationRepository
                .Setup(destinationRepository => destinationRepository
                .UpdateAsync(It.IsAny<Destination>()))
                .Callback(() => { });

            // Act
            var result = await destinationController.Update(mockDestinationDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<ResponseDestinationDto>(result.Value);
            Assert.Equal(mockDestinationList.ElementAt(0).Id, result.Value.GetType().GetProperty("Id").GetValue(result.Value));
        }

        [Fact]
        public async Task Update_ThrowsNotFoundException_WhenProvidedInvalidDestinationDto()
        {
            // Arrange
            var mockDestinationDto = MockDtoEntities<DestinationDto>.PopulateEntity();

            mockDestinationRepository
                .Setup(destinationRepository => destinationRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await destinationController.Update(mockDestinationDto));

            // Assert
            Assert.IsType<KeyNotFoundException>(result);
        }

        [Fact]
        public async Task Delete_OkObjectResult_WhenProvidedValidGuid()
        {
            // Arrange
            mockDestinationRepository
                .Setup(destinationRepository => destinationRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(mockDestinationList.ElementAt(0));

            // Act
            var result = await destinationController.Delete(new Guid());

            // Assert
            Assert.Equal(200, result.GetType().GetProperty("StatusCode").GetValue(result));
        }

        [Fact]
        public async Task Delete_ThrowsNotFoundException_WhenProvidedInvalidGuid()
        {
            // Arrange
            mockDestinationRepository
                .Setup(destinationRepository => destinationRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await destinationController.Delete(new Guid()));

            // Assert
            Assert.IsType<KeyNotFoundException>(result);
        }
    }
}
