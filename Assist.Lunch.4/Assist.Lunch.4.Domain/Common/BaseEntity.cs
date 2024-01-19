using System.ComponentModel.DataAnnotations;

namespace Assist.Lunch._4.Domain.Common
{
    public record BaseEntity
    {
        [Required]
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
