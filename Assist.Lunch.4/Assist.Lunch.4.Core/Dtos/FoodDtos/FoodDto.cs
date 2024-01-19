using Assist.Lunch._4.Core.Dtos.RestaurantDtos;

namespace Assist.Lunch._4.Core.Dtos.FoodDtos
{
    public class FoodDto : BaseFoodDto
    {
        public Guid Id { get; set; }
        public RestaurantDto? Restaurant { get; set; }
    }
}
