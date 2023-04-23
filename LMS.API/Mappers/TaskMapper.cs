using System.Linq;
using LMS.API.Models;
using LMS.API.Models.DTO;

namespace LMS.API.Mappers
{
    public static class TaskMapper
    {
        public static void SetValues(this Task task, UpdateTaskDTO updateTaskDTO)
        {
            task.Name = updateTaskDTO.Name;
            task.Description = updateTaskDTO.Description;
            task.MaxScore = updateTaskDTO.MaxScore;
            task.StartDate = updateTaskDTO.StartDate;
            task.EndDate = updateTaskDTO.EndDate;
            task.Status = updateTaskDTO.Status;
        }

        public static TaskDTO ToDto(this Task task) =>
            new TaskDTO
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                CreatedDate = task.CreatedDate,
                StartDate = task.StartDate,
                EndDate = task.EndDate,
                Status = task.Status,
                MaxScore = task.MaxScore,
            };
    }
}
