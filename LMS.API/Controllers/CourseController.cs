using System;
using System.Threading.Tasks;
using LMS.API.Context;
using LMS.API.Models;
using LMS.API.Models.DTO;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CourseController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDTO createCourseDTO)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var course = createCourseDTO.Adapt<Course>();
            course.Key = Guid.NewGuid().ToString();

            await _applicationDbContext.Courses.AddAsync(course);
            await _applicationDbContext.SaveChangesAsync();
            return Ok(course);
        }

        [HttpGet]
        public async Task<IActionResult> GetCourses() 
        {
            var courses = await _applicationDbContext.Courses.ToListAsync();

            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(Guid id)
        {
            if (!await _applicationDbContext.Courses.AnyAsync(course => course.Id == id))
                return NotFound();

            var course = await _applicationDbContext.Courses.FirstOrDefaultAsync(course => course.Id == id);

            if (course is null)
                return NotFound();

            return Ok(course);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] UpdateCourseDTO updateCourseDTO)
        {
            if(! await _applicationDbContext.Courses.AnyAsync(course => course.Id == id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var course = await _applicationDbContext.Courses.FirstOrDefaultAsync(course => course.Id == id);
            
            if (course is null)
                return NotFound();

            course.Name = updateCourseDTO.Name;

            await _applicationDbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var course = await _applicationDbContext.Courses.FirstOrDefaultAsync(course => course.Id == id);

            if (course is null)
                return NotFound();

            _applicationDbContext.Courses.Remove(course);
            await _applicationDbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
