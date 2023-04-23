using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.API.Models
{
    public class UserTask
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
        public Guid TaskId { get; set; }
        [ForeignKey(nameof(TaskId))]
        public virtual Task? Task { get; set; }
        public string? Description { get; set; }

        public EUserTaskStatus Status { get; set; }
    }
}
