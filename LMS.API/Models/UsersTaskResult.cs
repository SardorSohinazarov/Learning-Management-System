using System;

namespace LMS.API.Models
{
    public class UsersTaskResult : UserTaskResult
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }
}
