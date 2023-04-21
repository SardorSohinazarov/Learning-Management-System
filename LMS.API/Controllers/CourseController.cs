using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.API.Context;
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
    public class CourseController : ControllerBase
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(Guid id)
        {
            if (!await _applicationDbContext.Courses.AnyAsync(course => course.Id == id))
                return NotFound();

            var course = await _applicationDbContext.Courses.FirstOrDefaultAsync(course => course.Id == id);

            if (course is null)
                return NotFound();

            return Ok(course?.ToDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] UpdateCourseDTO updateCourseDTO)
        {

            if (!ModelState.IsValid)
                return BadRequest();

            var course = await _applicationDbContext.Courses.FirstOrDefaultAsync(course => course.Id == id);

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
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var course = await _applicationDbContext.Courses.FirstOrDefaultAsync(course => course.Id == id);

            if (course is null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            _applicationDbContext.Courses.Remove(course);
            await _applicationDbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
