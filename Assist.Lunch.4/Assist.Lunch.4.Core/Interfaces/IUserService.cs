using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.UserDtos;
using Assist.Lunch._4.Core.Models;
using Assist.Lunch._4.Domain.Entitites;

namespace Assist.Lunch._4.Core.Interfaces
{
    public interface IUserService
    {
        Task<MessageDto> Delete(Guid userId);
        Task<ResponseDto<ResponseUserDto>> GetAll();
        Task<User> Login(RequestToken requestToken);
        Task<MessageDto> SetAdmin(Guid userId);
        Task<ResponseUserDto> Register(RegisterUserDto registerUserDto);
        Task<ResponseUserDto> GetById(Guid id);
        Task<ResponseUserDto> Update(UserDto userDto);
        Task<MessageDto> UpdatePassword(UpdateUserPasswordDto updateUserPasswordDto);
    }
}
