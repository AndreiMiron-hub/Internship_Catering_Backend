using Assist.Lunch._4.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Assist.Lunch._4.Domain.Entitites
{
    public record Restaurant : BaseEntity
    {
        [Required]
        public string? Name { get; set; }
        public bool IsAvailable { get; set; }
        public ICollection<Food>? Foods { get; set; }
    }
}
