using System;

namespace LMS.API.Models.DTO
{
    public class TaskDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxScore { get; set; }
        public ETaskStatus Status { get; set; }
    }
}
