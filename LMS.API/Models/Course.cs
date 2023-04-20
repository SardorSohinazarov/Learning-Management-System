using System;

namespace LMS.API.Models
{
    public class Course
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Key { get; set; }
    }
}
