using System.ComponentModel.DataAnnotations;

namespace LMS.API.Models.DTO
{
    public class SignInUserDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
