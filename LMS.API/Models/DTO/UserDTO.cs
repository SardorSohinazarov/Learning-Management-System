using System.ComponentModel.DataAnnotations;

namespace LMS.API.Models.DTO
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
