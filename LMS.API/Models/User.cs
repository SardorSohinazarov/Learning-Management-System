using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace LMS.API.Models
{
    public class User : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public virtual List<UserCourse>? Courses { get; set; }
    }
}
