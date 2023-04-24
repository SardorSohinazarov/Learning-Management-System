using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.API.Context;
using LMS.API.Filters;
using LMS.API.Mappers;
using LMS.API.Models;
using LMS.API.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [TypeFilter(typeof(IsCourseExistsActionFilterAttribute))]
    [TypeFilter(typeof(IsTaskExistsActionFilterAttribute))]
    public partial class CourseController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<User> _userManager;

        public CourseController(
            ApplicationDbContext applicationDbContext,
            UserManager<User> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDTO createCourseDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.GetUserAsync(User);

            var course = new Course
            {
                Name = createCourseDTO.Name,
                Key = Guid.NewGuid().ToString(),
                Users = new List<UserCourse>
                {
                    new UserCourse
                    {
                        UserId = user.Id,
                        IsAdmin = true
                    }
                }
            };

            await _applicationDbContext.Courses.AddAsync(course);
            await _applicationDbContext.SaveChangesAsync();
            course = await _applicationDbContext.Courses.FirstOrDefaultAsync(c => c.Id == course.Id);

            return Ok(course.ToDto());
        }

        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            var courses = await _applicationDbContext.Courses.ToListAsync();
            List<CourseDTO> courseDTO = courses?.Select(course => course.ToDto()).ToList();

            return Ok(courseDTO);
        }

        [HttpGet("{courseId}")]
        [IsCourseUserOrAdmin(true)]
        public async Task<IActionResult> GetCourseById(Guid courseId)
        {
            if (!await _applicationDbContext.Courses.AnyAsync(course => course.Id == courseId))
                return NotFound();

            var course = await _applicationDbContext.Courses.FirstOrDefaultAsync(course => course.Id == courseId);

            if (course is null)
                return NotFound();

            return Ok(course?.ToDto());
        }

        [HttpPut("{courseId}")]
        public async Task<IActionResult> UpdateCourse(Guid courseId, [FromBody] UpdateCourseDTO updateCourseDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var course = await _applicationDbContext.Courses.FirstOrDefaultAsync(course => course.Id == courseId);

            if (course is null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);

            if (course.Users?.Any(uc => uc.UserId == user.Id && uc.IsAdmin) == false)
                return BadRequest();

            course.Name = updateCourseDTO.Name;
            await _applicationDbContext.SaveChangesAsync();

            return Ok(course?.ToDto());
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(Guid courseId)
        {
            var course = await _applicationDbContext.Courses.FirstOrDefaultAsync(course => course.Id == courseId);

            if (course is null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);

            if (course.Users?.Any(uc => uc.UserId == user.Id && uc.IsAdmin) == false)
                return BadRequest();

            _applicationDbContext.Courses.Remove(course);
            await _applicationDbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("{courseId}/join")]
        public async Task<IActionResult> JoinCourse(Guid courseId, [FromBody] JoinCourseDto joinCourseDto)
        {
            var course = await _applicationDbContext.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
            if (course is null)
                return NotFound();

            if (course.Key != joinCourseDto.CourseKey)
                return BadRequest();

            var user = await _userManager.GetUserAsync(User);
            if (course.Users?.Any(uc => uc.UserId == user.Id) == true)
                return BadRequest();

            if (course.Users?.Any(uc => uc.UserId == user.Id) == true)
                return BadRequest();

            await _applicationDbContext.UserCourses.AddAsync(
                new UserCourse
                {
                    UserId = user.Id,
                    CourseId = course.Id,
                    IsAdmin = false
                });

            await _applicationDbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{courseId}/leave")]
        public async Task<IActionResult> LeaveCourse(Guid courseId, [FromBody] LeaveCourseDTO leaveCourseDTO)
        {
            var course = await _applicationDbContext.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
            if (course is null)
                return NotFound();

            if (course.Key != leaveCourseDTO.CourseKey)
                return BadRequest();

            var user = await _userManager.GetUserAsync(User);
            if (course.Users?.Any(uc => uc.UserId == user.Id) == false)
                return BadRequest();

            var usercouerse = await _applicationDbContext.UserCourses
                .FirstOrDefaultAsync(uc => uc.UserId == user.Id & uc.CourseId == course.Id);

            _applicationDbContext.UserCourses.Remove(usercouerse);
            await _applicationDbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
