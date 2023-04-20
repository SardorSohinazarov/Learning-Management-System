using System.ComponentModel.DataAnnotations;

namespace LMS.API.Models.DTO
{
    public class CreateCourseDTO
    {
        [Required]
        public string? Name { get; set; }
    }
}
