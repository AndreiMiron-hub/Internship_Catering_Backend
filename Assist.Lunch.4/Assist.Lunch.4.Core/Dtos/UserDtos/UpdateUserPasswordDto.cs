namespace Assist.Lunch._4.Core.Dtos.UserDtos
{
    public class UpdateUserPasswordDto
    {
        public Guid Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
