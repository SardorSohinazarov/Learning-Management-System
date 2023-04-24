using System;
using System.Threading.Tasks;
using LMS.API.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Filters
{
    public class IsCourseExistsActionFilterAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext _context;

        public IsCourseExistsActionFilterAttribute(ApplicationDbContext context) =>
            _context = context;

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ActionArguments.ContainsKey("courseId"))
            {
                await next();
                return;
            }

            var courseId = (Guid)context.ActionArguments["courseId"];

            if(!await _context.Courses.AnyAsync(course => course.Id == courseId))
            {
                context.Result = new NotFoundResult();
                return;
            }

            await next();
        }
    }
}
