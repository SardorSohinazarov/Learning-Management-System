using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.API.Context;
using LMS.API.Filters;
using LMS.API.Mappers;
using LMS.API.Models;
using LMS.API.Models.DTO;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;

        public ProfileController(
            UserManager<User> userManager,
            ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet("courses")]
        public async Task<IActionResult> GetCourses()
        {
            var user = await _userManager.GetUserAsync(User);
            List<CourseDTO> courseDto = user.Courses.Select(usercourse => usercourse.Course.ToDto()).ToList();

            return Ok(courseDto);
        }

        [HttpGet("courses/{courseId}/tasks")]
        public async Task<IActionResult> GetTasks(Guid courseId)
        {
            var user = await _userManager.GetUserAsync(User);
            var course = await _applicationDbContext.Courses.FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
                return NotFound();

            var tasks = course.Tasks;
            var userTasks = new List<UserTaskResultDTO>();

            foreach (var task in tasks)
            {
                var result = task.Adapt<UserTaskResultDTO>();
                var userResultEntity = task.UserTasks?.FirstOrDefault(ut => ut.UserId == user.Id);

                result.UserResult = userResultEntity == null ? null : new UserTaskResult()
                {
                    Status = userResultEntity.Status,
                    Description = userResultEntity.Description,
                };

                userTasks.Add(result);
            }

            return Ok(userTasks);
        }

        [HttpPost("courses/{courseId}/tasks/{taskId}")]
        public async Task<IActionResult> AddUserTaskResult(Guid courseId, Guid taskId, [FromBody] CreateUserTaskResultDto resultDto)
        {
            var task = await _applicationDbContext.Tasks
                .FirstOrDefaultAsync(t => t.CourseId == courseId && t.Id == taskId);

            if (task is null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);

            var userTaskresult = await _applicationDbContext.UserTasks
                .FirstOrDefaultAsync(ut => ut.UserId == user.Id && ut.TaskId == taskId);

            if (userTaskresult is null)
            {
                userTaskresult = new UserTask
                {
                    UserId = user.Id,
                    TaskId = taskId,
                };

                await _applicationDbContext.UserTasks.AddAsync(userTaskresult);
            }

            if (task.Course.Users.Any(u => u.IsAdmin && u.UserId == user.Id))
            {
                if (userTaskresult.Status == EUserTaskStatus.Completed
                    && resultDto.Status is EUserTaskStatus.Accepted or EUserTaskStatus.Rejected)
                {
                    userTaskresult.Status = resultDto.Status;
                }
            }

            userTaskresult.Description = resultDto.Description;
            userTaskresult.Status = resultDto.Status;
            await _applicationDbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
