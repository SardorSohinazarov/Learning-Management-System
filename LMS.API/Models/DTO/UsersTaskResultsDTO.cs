using System.Collections.Generic;

namespace LMS.API.Models.DTO
{
    public class UsersTaskResultsDTO: TaskDTO
    {
        public List<UsersTaskResult>? UsersResult { get; set; }
    }
}
