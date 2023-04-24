using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Filters
{
    public class IsCourseUserOrAdminAttribute : TypeFilterAttribute
    {
        public bool OnlyAdmin { get; set; }

        public IsCourseUserOrAdminAttribute(bool onlyAdmin = false)
            : base(typeof(CourseAdminFilterAttribute))
        {
            Arguments = new object[] { onlyAdmin };
        }
    }
}
