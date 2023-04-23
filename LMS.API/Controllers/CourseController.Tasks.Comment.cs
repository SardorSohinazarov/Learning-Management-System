using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.API.Models;
using LMS.API.Models.DTO;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Controllers
{
    public partial class CourseController
    {
        [HttpGet("{courseId}/tasks/{taskId}/comments")]
        public async Task<IActionResult> GetTaskComments(Guid courseId, Guid taskId)
        {
            var task = await _applicationDbContext.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.CourseId == courseId);
           
            if (task == null)
                return NotFound();

            var comments = new List<TaskCommentDTO>();

            if (task.Comments is null)
                return Ok(comments);

            var mainComments = task.Comments.Where(tc => tc.ParentId == null).ToList();

            foreach (var comment in mainComments)
            {
                var commentDTO = ToTaskCommentDto(comment);
                comments.Add(commentDTO);
            }

            return Ok(comments);
        }

        [HttpPost("{courseId}/tasks/{taskId}/comments")]
        public async Task<IActionResult> AddTaskComments(
            Guid courseId,
            Guid taskId,
            [FromBody] CreateTaskCommentDTO createTaskCommentDTO)
        {
            var task = await _applicationDbContext.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.CourseId == courseId);

            if (task == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            task.Comments ??= new List<TaskComment>();

            task.Comments.Add(new TaskComment
            {
                TaskId = taskId,
                UserId = user.Id,
                Comment = createTaskCommentDTO.Comment,
                ParentId = createTaskCommentDTO.ParentId
            });

            await _applicationDbContext.SaveChangesAsync();

            return Ok();
        }

        private TaskCommentDTO ToTaskCommentDto(TaskComment comment)
        {
            var commentDto = new TaskCommentDTO()
            {
                Id = comment.Id,
                Comment = comment.Comment,
                CreatedDate = comment.CreatedDate,
                User = comment.User?.Adapt<UserDTO>(),
            };

            if (comment.Children is null)
                return commentDto;

            foreach (var child in comment.Children)
            {
                commentDto.Children ??= new List<TaskCommentDTO>();
                commentDto.Children.Add(ToTaskCommentDto(child));
            }

            return commentDto;
        }
    }
}
