using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.RestaurantDtos;
using Assist.Lunch._4.Domain.Entitites;
using AutoMapper;

namespace Assist.Lunch._4.Core.Helpers.Mapper.MappingProfiles
{
    public class RestaurantProfileMapper : Profile
    {
        public RestaurantProfileMapper()
        {
            CreateMap<Restaurant, RestaurantDto>();

            CreateMap<Restaurant, BaseRestaurantDto>();

            CreateMap<Restaurant, ResponseRestaurantDto>();

            CreateMap<BaseRestaurantDto, Restaurant>();

            CreateMap<IEnumerable<Restaurant>, ResponseDto<ResponseRestaurantDto>>()
                .ForMember(dest => dest.Records, opt => opt.MapFrom(source => source));
        }
    }
}
