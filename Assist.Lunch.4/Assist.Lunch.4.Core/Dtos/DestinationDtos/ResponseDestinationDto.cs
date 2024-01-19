using Assist.Lunch._4.Core.Dtos.CommonDtos;

namespace Assist.Lunch._4.Core.Dtos.DestinationDtos
{
    public class ResponseDestinationDto : BaseEntityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
