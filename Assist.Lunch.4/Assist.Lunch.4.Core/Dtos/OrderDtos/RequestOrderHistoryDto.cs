namespace Assist.Lunch._4.Core.Dtos.OrderDtos
{
    public class RequestOrderHistoryDto
    {
        public Guid UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
