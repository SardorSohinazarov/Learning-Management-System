namespace LMS.API.Models.DTO
{
    public class CreateUserTaskResultDto
    {
        public string? Description { get; set; }
        public EUserTaskStatus Status { get; set; }
    }
}
