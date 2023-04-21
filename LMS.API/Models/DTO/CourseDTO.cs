using System;
using System.Collections.Generic;

namespace LMS.API.Models.DTO
{
    public class CourseDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Key { get; set; }

        public List<UserDTO?>? Users { get; set; }
    }
}
