using Assist.Lunch._4.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Assist.Lunch._4.Domain.Entitites
{
    public record User : BaseEntity
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [MinLength(8)]
        [Required]
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
