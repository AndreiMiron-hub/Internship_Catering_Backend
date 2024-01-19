using Assist.Lunch._4.Core.Dtos.CommonDtos;

namespace Assist.Lunch._4.Core.Dtos.RestaurantDtos
{
    public class ResponseRestaurantDto : BaseEntityDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public bool IsAvailable { get; set; }
    }
}
