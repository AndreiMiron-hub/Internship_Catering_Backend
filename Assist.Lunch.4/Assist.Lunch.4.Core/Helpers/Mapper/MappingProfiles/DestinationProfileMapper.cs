using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.DestinationDtos;
using Assist.Lunch._4.Domain.Entitites;
using AutoMapper;

namespace Assist.Lunch._4.Core.Helpers.Mapper.MappingProfiles
{
    public class DestinationProfileMapper : Profile
    {
        public DestinationProfileMapper()
        {
            CreateMap<BaseDestinationDto, Destination>();

            CreateMap<Destination, DestinationDto>();

            CreateMap<Destination, ResponseDestinationDto>();

            CreateMap<DestinationDto, Destination>();

            CreateMap<IEnumerable<Destination>, ResponseDto<ResponseDestinationDto>>()
                .ForMember(dest => dest.Records, opt => opt.MapFrom(source => source));
        }
    }
}
