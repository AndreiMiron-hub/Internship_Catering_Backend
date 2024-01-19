using Assist.Lunch._4.Core.Dtos.CommonDtos;

namespace Assist.Lunch._4.Core.Dtos.UserDtos
{
    public class ResponseUserDto : BaseEntityDto
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDeleted { get; set; }
    }
}
