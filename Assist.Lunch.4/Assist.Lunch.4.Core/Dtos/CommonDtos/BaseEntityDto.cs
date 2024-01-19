namespace Assist.Lunch._4.Core.Dtos.CommonDtos
{
    public class BaseEntityDto
    {
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
