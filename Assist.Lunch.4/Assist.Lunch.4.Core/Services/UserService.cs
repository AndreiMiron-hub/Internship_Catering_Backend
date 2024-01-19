using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.UserDtos;
using Assist.Lunch._4.Core.Helpers.Encrypter;
using Assist.Lunch._4.Core.Helpers.ExceptionHandler.CustomExceptions;
using Assist.Lunch._4.Core.Helpers.Messages;
using Assist.Lunch._4.Core.Interfaces;
using Assist.Lunch._4.Core.Models;
using Assist.Lunch._4.Core.Security.Utils;
using Assist.Lunch._4.Domain.Entitites;
using Assist.Lunch._4.Infrastructure.Interfaces;
using Assist.Lunch._4.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Assist.Lunch._4.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IPasswordManager passwordManager;
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContext;
        private readonly IJwtUtils jwtUtils;

        public UserService(IUserRepository userRepository,
            IMapper mapper,
            IPasswordManager passwordManager,
            IHttpContextAccessor httpContext,
            IJwtUtils jwtUtils)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.passwordManager = passwordManager;
            this.httpContext = httpContext;
            this.jwtUtils = jwtUtils;
        }

        public async Task<MessageDto> Delete(Guid userId)
        {
            var user = await userRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new KeyNotFoundException(UserResources.NotFound);
            }

            user.IsDeleted = true;

            await userRepository.UpdateAsync(user);

            return MessagesConstructor.ReturnMessage(UserResources.UserDeactivated,
                StatusCodes.Status200OK);
        }

        public async Task<ResponseDto<ResponseUserDto>> GetAll()
        {
            var users = await userRepository.GetAllAsync();

            return mapper.Map<ResponseDto<ResponseUserDto>>(users);
        }

        public async Task<ResponseUserDto> GetById(Guid id)
        {
            var user = await userRepository.GetByIdAsync(id);

            if (user is null)
            {
                throw new KeyNotFoundException(UserResources.NotFound);
            }

            return mapper.Map<ResponseUserDto>(user);
        }

        public async Task<User> Login(RequestToken requestToken)
        {
            var user = await userRepository.GetByEmailAsync(requestToken.Email);

            if (user is null)
            {
                throw new InvalidCredentialsException(UserResources.InvalidEmail);
            }

            if (user.IsDeleted)
            {
                throw new InvalidCredentialsException(UserResources.UserDeactivated);
            }

            if (!passwordManager.PasswordCheck(requestToken.Password, user.Password))
            {
                throw new InvalidCredentialsException(UserResources.InvalidPassword);
            }

            return user;
        }

        public async Task<MessageDto> SetAdmin(Guid userId)
        {
            var user = await userRepository.GetByIdAsync(userId);

            if (user is null)
            {
                throw new KeyNotFoundException(UserResources.NotFound);
            }

            user.IsAdmin = true;

            await userRepository.UpdateAsync(user);

            return MessagesConstructor.ReturnMessage(UserResources.UserDeactivated,
                StatusCodes.Status200OK);
        }

        public async Task<ResponseUserDto> Register(RegisterUserDto registerUserDto)
        {
            var user = await userRepository.GetByEmailAsync(registerUserDto.Email);

            if (user is not null)
            {
                throw new EntityAlreadyExistsException(UserResources.UserAlreadyAdded);
            }

            var newUser = mapper.Map<User>(registerUserDto);

            newUser.Password = passwordManager.Encrypt(registerUserDto.Password);

            newUser = await userRepository.InsertAsync(newUser);

            return mapper.Map<ResponseUserDto>(newUser);
        }

        public async Task<ResponseUserDto> Update(UserDto userDto)
        {
            jwtUtils.CheckForOwnership(httpContext, userDto.Id);

            var user = await userRepository.GetByIdAsync(userDto.Id);

            if (user is null)
            {
                throw new KeyNotFoundException(UserResources.NotFound);
            }

            user = mapper.Map<User>(userDto);

            await userRepository.UpdateAsync(user);

            return mapper.Map<ResponseUserDto>(user);
        }

        public async Task<MessageDto> UpdatePassword(UpdateUserPasswordDto updateUserPasswordDto)
        {
            jwtUtils.CheckForOwnership(httpContext, updateUserPasswordDto.Id);

            var user = await userRepository.GetByIdAsync(updateUserPasswordDto.Id);

            if (user is null)
            {
                throw new KeyNotFoundException(UserResources.NotFound);
            }

            user = passwordManager.UpdatePassword(user, updateUserPasswordDto);

            await userRepository.UpdateAsync(user);

            return MessagesConstructor.ReturnMessage(UserResources.PasswordUpdated,
                StatusCodes.Status200OK);
        }
    }
}
