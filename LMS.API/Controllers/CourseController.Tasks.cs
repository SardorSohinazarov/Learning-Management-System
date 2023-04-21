﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.API.Mappers;
using LMS.API.Models.DTO;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Controllers
{
    public partial class CourseController
    {
        [HttpPost("{courseId}/tasks")]
        public async Task<IActionResult> AddTask(Guid courseId, [FromBody] CreateTaskDTO createTaskDTO)
        {
            if(! ModelState.IsValid)
                return BadRequest();

            var course = await _applicationDbContext.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
            if(course is null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);

            if(course.Users.Any(course => course.UserId == user.Id && course.IsAdmin) is false)
                return BadRequest();

            var task = createTaskDTO.Adapt<Models.Task>();
            task.CreatedDate = DateTime.Now;
            task.CourseId = courseId;

            await _applicationDbContext.Tasks.AddAsync(task);
            await _applicationDbContext.SaveChangesAsync();

            return Ok(task.Adapt<TaskDTO>());
        }

        [HttpGet("{courseId}/tasks")]
        public async Task<IActionResult> GetTasks(Guid courseId)
        {
            var course = await _applicationDbContext.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
            if (course is null)
                return NotFound();

            var tasks = _applicationDbContext.Tasks?.Select(task => task.Adapt<TaskDTO>()).ToList();

            return Ok(tasks?? new List<TaskDTO>());
        }
        
        [HttpGet("{courseId}/tasks/{taskId}")]
        public async Task<IActionResult> GetTaskById(Guid courseId, Guid taskId)
        {
            var task = await _applicationDbContext.Tasks?.FirstOrDefaultAsync(t => t.Id == taskId && t.CourseId == courseId);
            if(task is null)
                return NotFound();

            return Ok(task.Adapt<TaskDTO>());
        }
        
        [HttpPut("{courseId}/tasks/{taskId}")]
        public async Task<IActionResult> UpdateTask(Guid courseId, Guid taskId,[FromBody] UpdateTaskDTO updateTaskDTO)
        {
            var task = await _applicationDbContext.Tasks?.FirstOrDefaultAsync(t => t.Id == taskId && t.CourseId == courseId);
            if(task is null)
                return NotFound();

            task.SetValues(updateTaskDTO);

            await _applicationDbContext.SaveChangesAsync();

            return Ok(task.Adapt<TaskDTO>());
        }
        
        [HttpDelete("{courseId}/tasks/{taskId}")]
        public async Task<IActionResult> DeleteTask(Guid courseId, Guid taskId)
        {
            var task = await _applicationDbContext.Tasks?.FirstOrDefaultAsync(t => t.Id == taskId && t.CourseId == courseId);
            if(task is null)
                return NotFound();
            
            _applicationDbContext.Tasks.Remove(task);
            await _applicationDbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
