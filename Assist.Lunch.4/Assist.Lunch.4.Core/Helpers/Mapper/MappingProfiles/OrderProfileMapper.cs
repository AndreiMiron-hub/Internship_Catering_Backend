using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.OrderDtos;
using Assist.Lunch._4.Core.Dtos.RestaurantDtos;
using Assist.Lunch._4.Domain.Entitites;
using AutoMapper;

namespace Assist.Lunch._4.Core.Helpers.Mapper.MappingProfiles
{
    public class OrderProfileMapper : Profile
    {
        public OrderProfileMapper()
        {
            CreateMap<Order, ResponseOrderDto>();

            CreateMap<Restaurant, RestaurantDto>();

            CreateMap<UpdateOrderDto, Order>();

            CreateMap<BaseOrderDto, Order>();

            CreateMap<IEnumerable<Order>, ResponseDto<ResponseOrderDto>>()
                .ForMember(dest => dest.Records, opt => opt.MapFrom(source => source));
        }
    }
}
