using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.UserDtos;
using Assist.Lunch._4.Core.Models;
using Assist.Lunch._4.Domain.Entitites;
using AutoMapper;

namespace Assist.Lunch._4.Core.Helpers.Mapper.MappingProfiles
{
    public class UserProfileMapper : Profile
    {
        public UserProfileMapper()
        {
            CreateMap<User, AuthenticateResponseDto>();

            CreateMap<User, RequestToken>();

            CreateMap<User, RegisterUserDto>();

            CreateMap<RegisterUserDto, User>();

            CreateMap<User, ResponseUserDto>();

            CreateMap<User, UserDto>();

            CreateMap<UserDto, User>();

            CreateMap<IEnumerable<User>, ResponseDto<ResponseUserDto>>()
                .ForMember(dest => dest.Records, opt => opt.MapFrom(source => source));
        }
    }
}
