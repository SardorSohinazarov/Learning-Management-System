using System.ComponentModel.DataAnnotations;

namespace LMS.API.Models
{
    public class LocalizedStringEntity
    {
        [Key]
        public string? Key { get; set; }
        public string? Uz { get; set; }
        public string? Ru { get; set; }
        public string? En { get; set; }
    }
}
