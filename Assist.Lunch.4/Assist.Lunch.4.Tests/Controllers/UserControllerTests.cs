using Assist.Lunch._4.Controllers;
using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.UserDtos;
using Assist.Lunch._4.Core.Helpers.Encrypter;
using Assist.Lunch._4.Core.Helpers.ExceptionHandler.CustomExceptions;
using Assist.Lunch._4.Core.Helpers.Mapper.MappingProfiles;
using Assist.Lunch._4.Core.Security.Utils;
using Assist.Lunch._4.Core.Services;
using Assist.Lunch._4.Domain.Entitites;
using Assist.Lunch._4.Infrastructure.Interfaces;
using Assist.Lunch._4.Resources;
using Assist.Lunch._4.Tests.Mocks.MockDtos;
using Assist.Lunch._4.Tests.Mocks.MockEntities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Moq;

namespace Assist.Lunch._4.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IPasswordManager> mockPasswordManager;
        private readonly Mock<IUserRepository> mockUserRepository;
        private readonly Mock<IHttpContextAccessor> mockHttpContext;
        private readonly Mock<IJwtUtils> mockJwtUtils;
        private readonly IMapper mapper;
        private readonly UserController userController;
        private readonly UserService userService;
        private readonly IEnumerable<User> mockUserList;

        public UserControllerTests()
        {
            mockPasswordManager = new Mock<IPasswordManager>();
            mockUserRepository = new Mock<IUserRepository>();
            mockHttpContext = new Mock<IHttpContextAccessor>();
            mockJwtUtils = new Mock<IJwtUtils>();

            mapper = new MapperConfiguration(cfg => cfg.AddProfile<UserProfileMapper>()).CreateMapper();

            mockUserList = MockUserEntity.PopulateUserList();

            userService = new UserService(
                mockUserRepository.Object,
                mapper,
                mockPasswordManager.Object,
                mockHttpContext.Object,
                mockJwtUtils.Object);

            userController = new UserController(userService);
        }

        [Fact]
        public async Task Get_ReturnsOkActionResult_WhenDbTableIsNotEmpty()
        {
            // Arrange
            mockUserRepository
                .Setup(mockUserRepository => mockUserRepository
                .GetAllAsync())
                .ReturnsAsync(mockUserList);

            // Act
            var result = await userController.Get() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<ResponseDto<ResponseUserDto>>(result.Value);
            Assert.Equal(mockUserList.Count(), ((ResponseDto<ResponseUserDto>)result.Value).Records.Count());
        }

        [Fact]
        public async Task Get_ReturnOkActionResult_WhenProvidedValidGuid()
        {
            // Arrange
            mockUserRepository
                .Setup(mockUserRepository => mockUserRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid id) => mockUserList.FirstOrDefault(user => user.Id == id));

            // Act
            var result = await userController.Get(mockUserList.First().Id) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<ResponseUserDto>(result.Value);
            Assert.Equal(mockUserList.First().Id, result.Value.GetType().GetProperty("Id").GetValue(result.Value));
        }

        [Fact]
        public async Task Get_ThrowsNotFoundException_WhenProvidedInvalidGuid()
        {
            // Arrange
            mockUserRepository
                .Setup(mockUserRepository => mockUserRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await userController.Get(new Guid()));

            //Assert
            Assert.IsType<KeyNotFoundException>(result);
        }

        [Fact]
        public async Task Update_ReturnOkActionResult_WhenProvidedValidUserDto()
        {
            // Arrange
            var userDto = MockDtoEntities<UserDto>.PopulateEntity();
            userDto.Id = mockUserList.First().Id;

            mockUserRepository
               .Setup(mockUserRepository => mockUserRepository
               .GetByIdAsync(It.IsAny<Guid>()))
               .ReturnsAsync(mockUserList.First());

            mockUserRepository
                .Setup(mockUserRepository => mockUserRepository
                .UpdateAsync(mockUserList.First()))
                .ReturnsAsync(mockUserList.First());

            mockHttpContext
                .Setup(o => o
                .HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(It.IsAny<string>());

            mockJwtUtils
                .Setup(jwt => jwt
                .CheckForOwnership(It.IsAny<IHttpContextAccessor>(), It.IsAny<Guid>()));

            // Act
            var result = await userController.Update(userDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<ResponseUserDto>(result.Value);
            Assert.Equal(mockUserList.First().Id, result.Value.GetType().GetProperty("Id").GetValue(result.Value));
        }

        [Fact]
        public async Task Update_ThrowsInvalidCredentilsException_WhenActiveUserIsNotAccountOwnerOrAdmin()
        {
            // Arrange
            var userDto = MockDtoEntities<UserDto>.PopulateEntity();

            mockUserRepository
               .Setup(mockUserRepository => mockUserRepository
               .GetByIdAsync(It.IsAny<Guid>()))
               .ReturnsAsync(mockUserList.First());

            mockHttpContext
                .Setup(o => o
                .HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(It.IsAny<string>());

            mockJwtUtils
                .Setup(jwt => jwt
                .CheckForOwnership(It.IsAny<IHttpContextAccessor>(), It.IsAny<Guid>()))
                .Throws(new InvalidCredentialsException(UserResources.Unauthorized));

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await userController.Update(userDto));

            //Assert
            Assert.IsType<InvalidCredentialsException>(result);
        }

        [Fact]
        public async Task Update_ThrowsNotFoundException_WhenProvidedInvalidUser()
        {
            // Arrange
            var userDto = MockDtoEntities<UserDto>.PopulateEntity();

            mockUserRepository
                .Setup(mockUserRepository => mockUserRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            mockUserRepository
                .Setup(mockUserRepository => mockUserRepository
                .UpdateAsync(mockUserList.First()))
                .ReturnsAsync(mockUserList.First());

            mockHttpContext
                .Setup(o => o.HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(It.IsAny<string>());

            mockJwtUtils
                .Setup(jwt => jwt
                .CheckForOwnership(It.IsAny<IHttpContextAccessor>(), It.IsAny<Guid>()));

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await userController.Update(userDto));

            //Assert
            Assert.IsType<KeyNotFoundException>(result);
        }

        [Fact]
        public async Task Delete_OkObjectResult_WhenProvidedValidGuid()
        {
            // Arrange
            mockUserRepository
                .Setup(mockUserRepository => mockUserRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(mockUserList.First());

            mockUserRepository
                .Setup(mockUserRepository => mockUserRepository
                .DeleteAsync(mockUserList.First()));

            // Act
            var result = await userController.Delete(new Guid());

            // Assert
            Assert.Equal(200, result.GetType().GetProperty("StatusCode").GetValue(result));
        }

        [Fact]
        public async Task Delete_ThrowsNotFoundException_WhenProvidedInvalidGuid()
        {
            // Arrange
            mockUserRepository
                .Setup(mockUserRepository => mockUserRepository
                .GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => null);

            mockUserRepository
                .Setup(mockUserRepository => mockUserRepository
                .DeleteAsync(mockUserList.First()));

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await userController.Delete(new Guid()));

            //Assert
            Assert.IsType<KeyNotFoundException>(result);
        }

        [Fact]
        public async Task SetAdmin_ReturnOkResult_WhenProvidedValidGuid()
        {
            // Arrange
            mockUserRepository
               .Setup(mockUserRepository => mockUserRepository
               .GetByIdAsync(It.IsAny<Guid>()))
               .ReturnsAsync(mockUserList.First());

            mockUserRepository
                .Setup(mockUserRepository => mockUserRepository
                .UpdateAsync(mockUserList.First()))
                .ReturnsAsync(mockUserList.First());

            // Act
            var result = await userController.SetAdmin(new Guid()) as OkObjectResult;

            // Assert
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task SetAdmin_ThrowsNotFoundException_WhenProvidedInvalidGuid()
        {
            // Arrange
            mockUserRepository
               .Setup(mockUserRepository => mockUserRepository
               .GetByIdAsync(It.IsAny<Guid>()))
               .ReturnsAsync(() => null);

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await userController.SetAdmin(new Guid()));

            //Assert
            Assert.IsType<KeyNotFoundException>(result);
        }
    }
}
