using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.API.Models
{
    public class Task
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? MaxScore { get; set; }
        public ETaskStatus? Status { get; set; }

        public Guid? CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public virtual Course? Course { get; set; }
        public virtual List<UserTask>? UserTasks { get; set; }
        public virtual List<TaskComment>? Comments { get; set; }
    }
}
