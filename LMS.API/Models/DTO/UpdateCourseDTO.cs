using System.ComponentModel.DataAnnotations;

namespace LMS.API.Models.DTO
{
    public class UpdateCourseDTO
    {
        [Required]
        public string? Name { get; set; }
    }
}
