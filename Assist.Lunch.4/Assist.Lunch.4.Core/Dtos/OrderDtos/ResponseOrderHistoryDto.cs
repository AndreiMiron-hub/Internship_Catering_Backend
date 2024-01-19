using Assist.Lunch._4.Core.Dtos.UserDtos;

namespace Assist.Lunch._4.Core.Dtos.OrderDtos
{
    public class ResponseOrderHistoryDto
    {
        public UserDto userDto { get; set; }
        public int FullMenusOrdered { get; set; } = 0;
        public int FitnessMenusOrdered { get; set; } = 0;
        public double TotalCost { get; set; } = 0;
    }
}
