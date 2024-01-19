using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.FoodDtos;
using Assist.Lunch._4.Domain.Entitites;
using AutoMapper;

namespace Assist.Lunch._4.Core.Helpers.Mapper.MappingProfiles
{
    public class FoodProfileMapper : Profile
    {
        public FoodProfileMapper()
        {
            CreateMap<Food, UpdateFoodDto>();

            CreateMap<UpdateFoodDto, Food>();

            CreateMap<AddFoodDto, Food>();

            CreateMap<Food, AddFoodDto>();

            CreateMap<Food, FoodDto>();

            CreateMap<FoodDto, Food>();

            CreateMap<Food, ResponseFoodDto>();

            CreateMap<IEnumerable<Food>, ResponseDto<ResponseFoodDto>>()
                .ForMember(dest => dest.Records, opt => opt.MapFrom(source => source));
        }
    }
}
