using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.DestinationDtos;
using Assist.Lunch._4.Core.Dtos.FoodDtos;
using Assist.Lunch._4.Core.Dtos.UserDtos;
using Assist.Lunch._4.Domain.Enums;

namespace Assist.Lunch._4.Core.Dtos.OrderDtos
{
    public class ResponseOrderDto : BaseEntityDto
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public UserDto? User { get; set; }
        public DestinationDto? Destination { get; set; }
        public List<FoodDto> Foods { get; set; } = new();
    }
}
