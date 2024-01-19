using Assist.Lunch._4.Controllers;
using Assist.Lunch._4.Core.Dtos.UserDtos;
using Assist.Lunch._4.Core.Helpers.Encrypter;
using Assist.Lunch._4.Core.Helpers.ExceptionHandler.CustomExceptions;
using Assist.Lunch._4.Core.Helpers.Mapper.MappingProfiles;
using Assist.Lunch._4.Core.Models;
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
    public class AuthenticationControllerTests
    {
        private readonly Mock<IPasswordManager> mockPasswordManager;
        private readonly Mock<IUserRepository> mockUserRepository;
        private readonly Mock<IHttpContextAccessor> mockHttpContext;
        private readonly Mock<IJwtUtils> mockJwtUtils;
        private readonly IMapper mapper;
        private readonly AuthenticationController authenticationController;
        private readonly UserService userService;
        private readonly AuthenticationService authenticationService;
        private readonly IEnumerable<User> mockUserList;

        public AuthenticationControllerTests()
        {
            mockPasswordManager = new Mock<IPasswordManager>();
            mockUserRepository = new Mock<IUserRepository>();
            mockHttpContext = new Mock<IHttpContextAccessor>();
            mockJwtUtils = new Mock<IJwtUtils>();

            mapper = new MapperConfiguration(cfg => cfg
                .AddProfile<UserProfileMapper>())
                .CreateMapper();

            mockUserList = MockUserEntity.PopulateUserList();

            userService = new UserService(
                mockUserRepository.Object,
                mapper,
                mockPasswordManager.Object,
                mockHttpContext.Object,
                mockJwtUtils.Object);

            authenticationService = new AuthenticationService(
                userService,
                mockJwtUtils.Object,
                mapper);

            authenticationController = new AuthenticationController(
                userService,
                authenticationService);
        }

        [Fact]
        public async Task Authenticate_ReturnsOkActionResult_WhenProvidedValidCredentials()
        {
            // Arrange
            var mockRequestToken = MockDtoEntities<RequestToken>.PopulateEntity();
            var mockAuthenticateResponseDto = MockDtoEntities<AuthenticateResponseDto>.PopulateEntity();

            mockUserRepository
                .Setup(userRepository => userRepository
                .GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(mockUserList.First());

            mockPasswordManager
                .Setup(passwordManager => passwordManager
                .PasswordCheck(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            mockJwtUtils
                .Setup(login => login
                .GenerateToken(mockUserList.First()))
                .Returns(It.IsAny<string>());

            // Act
            var result = await authenticationController.Authenticate(mockRequestToken) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<AuthenticateResponseDto>(result.Value);
            Assert.Equal(mockUserList.First().Id, result.Value.GetType().GetProperty("Id").GetValue(result.Value));
        }

        [Fact]
        public async Task Authenticate_ThrowsInvalidCredentialsException_WhenProvidedInvalidCredentials()
        {
            // Arrange
            var mockRequestToken = MockDtoEntities<RequestToken>.PopulateEntity();

            mockUserRepository
                .Setup(userRepository => userRepository
                .GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(mockUserList.First());

            mockPasswordManager
                .Setup(passwordManager => passwordManager
                .PasswordCheck(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            mockJwtUtils
                .Setup(login => login
                .GenerateToken(mockUserList.First()))
                .Returns(It.IsAny<string>());

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await authenticationController.Authenticate(mockRequestToken));

            // Assert
            Assert.IsType<InvalidCredentialsException>(result);
        }

        [Fact]
        public async Task Register_ReturnsOkActionResult_WhenProvidedValidNewUser()
        {
            // Arrange
            var mockRegisterUserDto = MockDtoEntities<RegisterUserDto>.PopulateEntity();
            var mockResponseUserDto = MockDtoEntities<ResponseUserDto>.PopulateEntity();

            mockUserRepository
                .Setup(userRepository => userRepository
                .GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            mockPasswordManager
                .Setup(passwordManager => passwordManager
                .Encrypt(It.IsAny<string>()))
                .Returns(mockUserList.First().Password);

            mockUserRepository
                .Setup(userRepository => userRepository
                .InsertAsync(It.IsAny<User>()))
                .ReturnsAsync(mockUserList.First());

            // Act
            var result = await authenticationController.Register(mockRegisterUserDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.IsAssignableFrom<ResponseUserDto>(result.Value);
            Assert.Equal(mockUserList.First().Id, result.Value.GetType().GetProperty("Id").GetValue(result.Value));
        }

        [Fact]
        public async Task Register_ThrowsEntityAlreadyExistsException_WhenProvidedExistingUser()
        {
            // Arrange
            var mockRegisterUserDto = MockDtoEntities<RegisterUserDto>.PopulateEntity();

            mockUserRepository
                .Setup(userRepository => userRepository
                .GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(mockUserList.First());

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await authenticationController.Register(mockRegisterUserDto));

            // Assert
            Assert.IsType<EntityAlreadyExistsException>(result);
        }

        [Fact]
        public async Task UpdatePassword_ReturnsOkActionResult_WhenProvidedValidUpdatePasswordDto()
        {
            // Arrange
            var mockUpdateUserPasswordDto = MockDtoEntities<UpdateUserPasswordDto>.PopulateEntity();
            var mockResponseUserDto = MockDtoEntities<ResponseUserDto>.PopulateEntity();

            mockUserRepository
               .Setup(mockUserRepository => mockUserRepository
               .GetByIdAsync(It.IsAny<Guid>()))
               .ReturnsAsync(mockUserList.First());

            mockPasswordManager
                .Setup(passwordManager => passwordManager
                .UpdatePassword(It.IsAny<User>(), It.IsAny<UpdateUserPasswordDto>()))
                .Returns(mockUserList.First());

            mockHttpContext
                .Setup(o => o
                .HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(It.IsAny<string>());

            mockJwtUtils
                .Setup(jwt => jwt
                .CheckForOwnership(It.IsAny<IHttpContextAccessor>(), It.IsAny<Guid>()));

            // Act
            var result = await authenticationController.UpdatePassword(mockUpdateUserPasswordDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task UpdatePassword_ThrowsNotFoundException_WhenProvidedInvalidGuid()
        {
            // Arrange
            var mockUpdateUserPasswordDto = MockDtoEntities<UpdateUserPasswordDto>.PopulateEntity();

            mockUserRepository
              .Setup(mockUserRepository => mockUserRepository
              .GetByIdAsync(It.IsAny<Guid>()))
              .ReturnsAsync(() => null);

            mockHttpContext
               .Setup(o => o
               .HttpContext.Request.Headers[HeaderNames.Authorization])
               .Returns(It.IsAny<string>());

            mockJwtUtils
                .Setup(jwt => jwt
                .CheckForOwnership(It.IsAny<IHttpContextAccessor>(), It.IsAny<Guid>()));

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await authenticationController.UpdatePassword(mockUpdateUserPasswordDto));

            //Assert
            Assert.IsType<KeyNotFoundException>(result);
        }

        [Fact]
        public async Task UpdatePassword_ThrowsInvalidCredentialsException_WhenProvidedInvalidCredentials()
        {
            // Arrange
            var mockUpdateUserPasswordDto = MockDtoEntities<UpdateUserPasswordDto>.PopulateEntity();

            mockUserRepository
              .Setup(mockUserRepository => mockUserRepository
              .GetByIdAsync(It.IsAny<Guid>()))
              .ReturnsAsync(mockUserList.First());

            mockPasswordManager
                .Setup(passwordManager => passwordManager
                .UpdatePassword(It.IsAny<User>(), It.IsAny<UpdateUserPasswordDto>()))
                .Throws(new InvalidCredentialsException());

            mockHttpContext
                .Setup(o => o
                .HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(It.IsAny<string>());

            mockJwtUtils
                .Setup(jwt => jwt
                .CheckForOwnership(It.IsAny<IHttpContextAccessor>(), It.IsAny<Guid>()));

            // Act
            var result = await Assert
                .ThrowsAnyAsync<Exception>(async () =>
                await authenticationController.UpdatePassword(mockUpdateUserPasswordDto));

            //Assert
            Assert.IsType<InvalidCredentialsException>(result);
        }
    }
}