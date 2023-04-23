using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.API.Models.DTO
{
    public class CreateTaskCommentDTO
    {
        [Required]
        public string? Comment { get; set; }
        public Guid? ParentId { get; set; }
    }
}
