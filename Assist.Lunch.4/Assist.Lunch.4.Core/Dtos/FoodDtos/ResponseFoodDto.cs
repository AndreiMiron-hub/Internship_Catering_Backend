using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.RestaurantDtos;
using Assist.Lunch._4.Domain.Enums;

namespace Assist.Lunch._4.Core.Dtos.FoodDtos
{
    public class ResponseFoodDto : BaseEntityDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Category Category { get; set; }
        public double Price { get; set; }
        public RestaurantDto? Restaurant { get; set; }
    }
}
