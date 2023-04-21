using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.API.Models
{
    public class UserCourse
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }

        public Guid CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public virtual Course? Course { get; set; }

        public bool IsAdmin { get; set; }
    }
}
