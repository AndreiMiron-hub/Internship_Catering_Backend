using Assist.Lunch._4.Domain.Enums;

namespace Assist.Lunch._4.Core.Dtos.FoodDtos
{
    public class BaseFoodDto
    {
        public string? Name { get; set; }
        public Category Category { get; set; }
        public double Price { get; set; }
    }
}
