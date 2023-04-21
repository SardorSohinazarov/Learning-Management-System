using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.API.Mappers;
using LMS.API.Models;
using LMS.API.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        public ProfileController(UserManager<User> userManager) =>
            _userManager = userManager;

        [HttpGet("courses")]
        public async Task<IActionResult> GetCourses()
        {
            var user = await _userManager.GetUserAsync(User);
            List<CourseDTO> courseDto = user.Courses.Select(usercourse => usercourse.Course.ToDto()).ToList();
            return Ok(courseDto);
        }
    }
}
