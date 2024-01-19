using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.DestinationDtos;
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
    public class DestinationServiceTests
    {
        private readonly Mock<IDestinationRepository> mockDestinationRepository;
        private readonly IMapper mapper;
        private readonly DestinationService destinationService;
        private readonly IEnumerable<Destination> mockDestinationList;

        public DestinationServiceTests()
        {
            mockDestinationRepository = new Mock<IDestinationRepository>();

            mapper = new MapperConfiguration(cfg => cfg.AddProfile<DestinationProfileMapper>()).CreateMapper();

            mockDestinationList = MockDestinationEntity.PopulateDestinationList();

            destinationService = new DestinationService(
                mockDestinationRepository.Object,
                mapper);
        }

        [Fact]
        public async Task GetAll_ReturnsDestinationList_WhenDbTableIsPopulated()
        {
            //Arrange
            mockDestinationRepository
                .Setup(destinationRepository => destinationRepository
                .GetAllAsync())
                .ReturnsAsync(mockDestinationList);

            //Act
            var result = destinationService.GetAll();

            //Assert
            await Assert.IsType<Task<ResponseDto<ResponseDestinationDto>>>(result);
            var destinations = await mockDestinationRepository.Object.GetAllAsync();
            Assert.Equal(mockDestinationList.Count(), destinations.Count());
        }

        [Fact]
        public async Task GetById_ReturnsDestination_WhenProvidedValidGuid()
        {
            // Arrange
            mockDestinationRepository
                .Setup(destinationRepository => destinationRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(mockDestinationList.FirstOrDefault(mockDestination => mockDestination.Id == mockDestinationList.First().Id));

            // Act 
            var result = destinationService.GetById(mockDestinationList.First().Id);

            // Assert
            await Assert.IsType<Task<ResponseDestinationDto>>(result);
        }

        [Fact]
        public async Task GetById_ThrowsNotFoundException_WhenProvidedInvalidGuid()
        {
            // Assert
            var ex = await Assert
                .ThrowsAsync<KeyNotFoundException>(() =>
                destinationService.GetById(new Guid()));
            Assert.Equal(ex.Message, DestinationResources.NotFound);
        }

        [Fact]
        public async Task Add_ReturnsResponseDestinationDto_WhenProvidedNewValidDestination()
        {
            // Arrange
            var mockBaseDestinationDto = MockDtoEntities<BaseDestinationDto>.PopulateEntity();
            var mockResponseDestinationDto = MockDtoEntities<ResponseDestinationDto>.PopulateEntity();

            mockDestinationRepository
                .Setup(destinationRepository => destinationRepository
                .GetByNameAsync(mockBaseDestinationDto.Name))
                .ReturnsAsync(() => null);

            mockDestinationRepository
                .Setup(destinationRepository => destinationRepository
                .InsertAsync(mockDestinationList.First()))
                .ReturnsAsync(mockDestinationList.First());

            // Act 
            var result = destinationService.Add(mockBaseDestinationDto);

            // Assert
            await Assert.IsType<Task<ResponseDestinationDto>>(result);
        }

        [Fact]
        public async Task Add_ThrowsEntityAlreadyExistsException_WhenProvidedExistingDestination()
        {
            // Arrange
            var mockBaseDestinationDto = MockDtoEntities<BaseDestinationDto>.PopulateEntity();
            var mockResponseDestinationDto = MockDtoEntities<ResponseDestinationDto>.PopulateEntity();

            mockDestinationRepository
                .Setup(destinationRepository => destinationRepository
                .GetByNameAsync(mockBaseDestinationDto.Name))
                .ReturnsAsync(mockDestinationList.First());

            // Act 
            var result = destinationService.Add(mockBaseDestinationDto);

            // Assert
            var ex = await Assert
                .ThrowsAsync<EntityAlreadyExistsException>(() =>
                destinationService.Add(mockBaseDestinationDto));
            Assert.Equal(ex.Message, DestinationResources.DestinationAlreadyAdded);
        }

        [Fact]
        public async Task Update_ReturnsResponseDestinationDto_WhenProvidedValidDestinationDto()
        {
            // Arrange
            var mockDestinationDto = MockDtoEntities<DestinationDto>.PopulateEntity();

            mockDestinationRepository
                .Setup(destinationRepository => destinationRepository
                .GetByIdAsync(mockDestinationDto.Id))
                .ReturnsAsync(mockDestinationList.First());

            // Act
            var result = destinationService.Update(mockDestinationDto);

            // Assert
            await Assert.IsType<Task<ResponseDestinationDto>>(result);
        }

        [Fact]
        public async Task Update_ThrowsNotFound_WhenProvidedInvalidDestinationDto()
        {
            // Arrange 
            var mockDestinationDto = MockDtoEntities<DestinationDto>.PopulateEntity();

            mockDestinationRepository
                .Setup(destinationRepository => destinationRepository
                .GetByIdAsync(mockDestinationDto.Id))
                .ReturnsAsync(() => null);

            // Act
            var result = destinationService.Update(mockDestinationDto);

            // Assert
            var ex = await Assert
                .ThrowsAsync<KeyNotFoundException>(() =>
                destinationService.Update(mockDestinationDto));
            Assert.Equal(ex.Message, DestinationResources.NotFound);
        }

        [Fact]
        public async Task Delete_ChangeProperty_WhenProvidedValidDestination()
        {
            // Arrange
            mockDestinationRepository
                .Setup(mockDestinationRepository => mockDestinationRepository
                .GetByIdAsync(mockDestinationList.First().Id))
                .ReturnsAsync(mockDestinationList.First());

            // Act
            var response = destinationService.Delete(mockDestinationList.First().Id);

            // Assert
            Assert.True(mockDestinationList.First().IsDeleted);
        }

        [Fact]
        public async Task Delete_ThrowsNotFound_WhenProvidedInvalidDestination()
        {
            // Arrange
            mockDestinationRepository
                .Setup(mockDestinationRepository => mockDestinationRepository
                .GetByIdAsync(mockDestinationList.First().Id))
                .ReturnsAsync(() => null);

            // Act
            var response = destinationService.Delete(mockDestinationList.First().Id);

            // Assert
            var ex = await Assert
                .ThrowsAsync<KeyNotFoundException>(() =>
                destinationService.Delete(mockDestinationList.First().Id));
            Assert.Equal(ex.Message, DestinationResources.NotFound);
        }
    }
}
