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
    }
}
