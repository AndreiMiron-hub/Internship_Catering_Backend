using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.UserDtos;
using Assist.Lunch._4.Core.Helpers.Encrypter;
using Assist.Lunch._4.Core.Helpers.ExceptionHandler.CustomExceptions;
using Assist.Lunch._4.Core.Helpers.Mapper.MappingProfiles;
using Assist.Lunch._4.Core.Models;
using Assist.Lunch._4.Core.Security.Utils;
using Assist.Lunch._4.Core.Services;
using Assist.Lunch._4.Domain.Entitites;
using Assist.Lunch._4.Infrastructure.Interfaces;
using Assist.Lunch._4.Resources;
using Assist.Lunch._4.Tests.Mocks.MockDtos;
using Assist.Lunch._4.Tests.Mocks.MockEntities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Moq;

namespace Assist.Lunch._4.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> mockUserRepository;
        private readonly Mock<IPasswordManager> mockPasswordManager;
        private readonly Mock<IHttpContextAccessor> MockHttpContext;
        private readonly Mock<IJwtUtils> mockJwtUtils;
        private readonly UserService userService;
        private readonly IEnumerable<User> mockUserList;
        private readonly IMapper mapper;

        public UserServiceTests()
        {
            mockUserRepository = new Mock<IUserRepository>();
            mockPasswordManager = new Mock<IPasswordManager>();
            MockHttpContext = new Mock<IHttpContextAccessor>();
            mockJwtUtils = new Mock<IJwtUtils>();
            mockUserList = MockUserEntity.PopulateUserList();

            mapper = new MapperConfiguration(cfg => cfg.AddProfile<UserProfileMapper>()).CreateMapper();

            userService = new UserService(
                mockUserRepository.Object,
                mapper,
                mockPasswordManager.Object,
                MockHttpContext.Object,
                mockJwtUtils.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsUsersList_WhenDbTableIsPopulated()
        {
            // Arrange 
            mockUserRepository
                .Setup(userRepository => userRepository
                .GetAllAsync())
                .ReturnsAsync(mockUserList);

            // Act
            var result = userService.GetAll();

            // Assert
            await Assert.IsType<Task<ResponseDto<ResponseUserDto>>>(result);
            var users = await mockUserRepository.Object.GetAllAsync();
            Assert.Equal(mockUserList.Count(), users.Count());
        }

        [Fact]
        public async Task GetById_ReturnsUser_WhenProvidedValidGuid()
        {
            // Arrange 
            mockUserRepository
                .Setup(userRepository => userRepository
                .GetByIdAsync(mockUserList.First().Id))
                .ReturnsAsync(mockUserList.First());

            // Act
            var result = userService.GetById(mockUserList.First().Id);

            // Assert
            await Assert.IsType<Task<ResponseUserDto>>(result);
        }

        [Fact]
        public async Task GetById_ThrowsNotFoundException_WhenProvidedInvalidGuid()
        {
            // Assert
            var ex = await Assert
                .ThrowsAsync<KeyNotFoundException>(() =>
                userService.GetById(new Guid()));
            Assert.Equal(ex.Message, UserResources.NotFound);
        }

        [Fact]
        public async Task LoginUserAsync_ReturnToken_WhenProvidedValidTokenRequest()
        {
            // Arrange 
            mockUserRepository
                .Setup(userRepository => userRepository
                .GetByEmailAsync(mockUserList.First().Email))
                .ReturnsAsync(mockUserList.First());

            mockPasswordManager
                .Setup(pm => pm
                .PasswordCheck(mockUserList.First().Password, mockUserList.First().Password))
                .Returns(true);

            RequestToken requestToken = new()
            {
                Email = mockUserList.First().Email,
                Password = mockUserList.First().Password
            };

            // Act
            var result = userService.Login(requestToken);

            // Assert
            await Assert.IsType<Task<User>>(result);
        }

        [Fact]
        public async Task LoginUserAsync_ThrowsInvalidCredentialsExceptionException_WhenProvidedInexistentEmail()
        {
            // Arrange 
            mockUserRepository
                .Setup(userRepository => userRepository
                .GetByEmailAsync(mockUserList.First().Email))
                .ReturnsAsync(() => null);

            RequestToken requestToken = new()
            {
                Email = mockUserList.First().Email,
                Password = mockUserList.First().Password
            };

            // Assert
            var ex = await Assert
                .ThrowsAsync<InvalidCredentialsException>(() =>
                userService.Login(requestToken));
            Assert.Equal(ex.Message, UserResources.InvalidEmail);
        }

        [Fact]
        public async Task LoginUserAsync_ThrowsInvalidCredentialsException_WhenProvidedAnInvalidUSer()
        {
            // Arrange 
            mockUserRepository
                .Setup(userRepository => userRepository
                .GetByEmailAsync(mockUserList.Last().Email))
                .ReturnsAsync(mockUserList.Last());

            mockPasswordManager
                .Setup(pm => pm
                .PasswordCheck("testPassword!", mockUserList.Last().Password))
                .Returns(true);

            RequestToken requestToken = new() { Email = mockUserList.Last().Email, Password = mockUserList.Last().Password };

            // Act
            var result = userService.Login(requestToken);

            // Assert
            var ex = await Assert
                .ThrowsAsync<InvalidCredentialsException>(() =>
                userService.Login(requestToken));
            Assert.Equal(ex.Message, UserResources.InvalidPassword);
        }

        [Fact]
        public async Task LoginUserAsync_ThrowsInvalidCredentialsExceptionException_WhenProvidedInvalidPassword()
        {
            // Arrange 
            mockUserRepository
                .Setup(userRepository => userRepository
                .GetByEmailAsync(mockUserList.First().Email))
                .ReturnsAsync(mockUserList.First());

            mockPasswordManager
                .Setup(pm => pm
                .PasswordCheck("testWrongPassword!", mockUserList.First().Password))
                .Returns(false);

            RequestToken requestToken = new() { Email = mockUserList.First().Email, Password = mockUserList.First().Password };

            // Act
            var result = userService.Login(requestToken);

            // Assert
            var ex = await Assert
                .ThrowsAsync<InvalidCredentialsException>(() =>
                userService.Login(requestToken));
            Assert.Equal(ex.Message, UserResources.InvalidPassword);
        }

        [Fact]
        public async Task Register_ReturnResponseUserDto_WhenProvidedValidNewUser()
        {
            // Arrange 
            var registerUser = MockDtoEntities<RegisterUserDto>.PopulateEntity();

            mockUserRepository
                .Setup(userRepository => userRepository
                .GetByEmailAsync(registerUser.Email))
                .ReturnsAsync(() => null);

            mockPasswordManager
                .Setup(pm => pm
                .Encrypt(registerUser.Password))
                .Returns("EncryptedPassword");

            // Act
            var result = userService.Register(registerUser);

            // Assert
            await Assert.IsType<Task<ResponseUserDto>>(result);
        }

        [Fact]
        public async Task Register_ThrowsUserAlreadyRegistered_WhenProvidedUserExists()
        {
            // Arrange 
            var registerUser = MockDtoEntities<RegisterUserDto>.PopulateEntity();

            mockUserRepository
                .Setup(userRepository => userRepository
                .GetByEmailAsync(registerUser.Email))
                .ReturnsAsync(mockUserList.First());
            // Act
            var result = userService.Register(registerUser);

            // Assert
            var ex = await Assert
                .ThrowsAsync<EntityAlreadyExistsException>(() =>
                userService.Register(registerUser));
            Assert.Equal(ex.Message, UserResources.UserAlreadyAdded);
        }

        [Fact]
        public async Task Update_ReturnUpdatedUser_WhenProvidedValidUser()
        {
            // Arrange 
            var userDto = MockDtoEntities<UserDto>.PopulateEntity();

            mockUserRepository
                .Setup(userRepository => userRepository
                .GetByIdAsync(userDto.Id))
                .ReturnsAsync(mockUserList.First());

            MockHttpContext
                .Setup(o => o
                .HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(It.IsAny<string>());

            mockJwtUtils
                .Setup(jwt => jwt
                .CheckForOwnership(It.IsAny<IHttpContextAccessor>(), It.IsAny<Guid>()));

            // Act
            var result = userService.Update(userDto);

            // Assert
            await Assert.IsType<Task<ResponseUserDto>>(result);
        }

        [Fact]
        public async Task UpdateUserInfo_ThrowsUserNotFound_WhenProvidedInvalidGuid()
        {
            // Arrange 
            var userDto = MockDtoEntities<UserDto>.PopulateEntity();

            mockUserRepository
                .Setup(userRepository => userRepository
                .GetByIdAsync(userDto.Id))
                .ReturnsAsync(() => null);

            MockHttpContext
                .Setup(o => o
                .HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(It.IsAny<string>());

            mockJwtUtils
                .Setup(jwt => jwt
                .CheckForOwnership(It.IsAny<IHttpContextAccessor>(), It.IsAny<Guid>()));

            // Act
            var result = userService.Update(userDto);

            // Assert
            var ex = await Assert
                .ThrowsAsync<KeyNotFoundException>(() =>
                userService.Update(userDto));
            Assert.Equal(ex.Message, UserResources.NotFound);
        }

        [Fact]
        public async Task UpdateUserPassword_ReturnUpdatedUser_WhenProvidedValidUser()
        {
            // Arrange 
            var updateUserPasswordDto = MockDtoEntities<UpdateUserPasswordDto>.PopulateEntity();

            mockUserRepository
                .Setup(userRepository => userRepository
                .GetByIdAsync(updateUserPasswordDto.Id))
                .ReturnsAsync(mockUserList.First());

            MockHttpContext
                .Setup(o => o
                .HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(It.IsAny<string>());

            mockJwtUtils
                .Setup(jwt => jwt
                .CheckForOwnership(It.IsAny<IHttpContextAccessor>(), It.IsAny<Guid>()));

            mockPasswordManager
                .Setup(mockPasswordManager => mockPasswordManager
                .UpdatePassword(mockUserList.First(), updateUserPasswordDto));

            // Act
            var result = userService.UpdatePassword(updateUserPasswordDto);

            // Assert
            await Assert.IsType<Task<MessageDto>>(result);
        }

        [Fact]
        public async Task UpdateUserPassword_ThrowsUserNotFound_WhenProvidedInvalidUser()
        {
            // Arrange 
            var updateUserPasswordDto = MockDtoEntities<UpdateUserPasswordDto>.PopulateEntity();

            mockUserRepository
                .Setup(userRepository => userRepository
                .GetByIdAsync(updateUserPasswordDto.Id))
                .ReturnsAsync(() => null);

            MockHttpContext
                .Setup(o => o
                .HttpContext.Request.Headers[HeaderNames.Authorization])
                .Returns(It.IsAny<string>());

            mockJwtUtils
                .Setup(jwt => jwt
                .CheckForOwnership(It.IsAny<IHttpContextAccessor>(), It.IsAny<Guid>()));

            mockPasswordManager
                .Setup(pm => pm
                .UpdatePassword(mockUserList.First(), updateUserPasswordDto));

            // Act
            var result = userService.UpdatePassword(updateUserPasswordDto);

            // Assert
            var ex = await Assert
                .ThrowsAsync<KeyNotFoundException>(() =>
                userService.UpdatePassword(updateUserPasswordDto));
            Assert.Equal(ex.Message, UserResources.NotFound);
        }
    }
}