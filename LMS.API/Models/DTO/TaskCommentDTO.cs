using System;
using System.Collections.Generic;

namespace LMS.API.Models.DTO
{
    public class TaskCommentDTO
    {
        public Guid Id { get; set; }
        public string? Comment { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual List<TaskCommentDTO>? Children { get; set; }
        public virtual UserDTO? User { get; set; }
    }
}
