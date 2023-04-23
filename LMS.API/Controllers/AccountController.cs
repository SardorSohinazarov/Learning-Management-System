using System.Threading.Tasks;
using Castle.Core.Logging;
using LMS.API.Models;
using LMS.API.Models.DTO;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace LMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger _logger;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpUserDTO createUserDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (createUserDTO.Password != createUserDTO.ConfirmPassword)
                return BadRequest();

            if (await _userManager.Users.AnyAsync(user => user.UserName == createUserDTO.UserName))
                return BadRequest();

            var user = createUserDTO.Adapt<User>();
            await _userManager.CreateAsync(user, createUserDTO.Password);

            _logger.LogInformation("User saved to database");
            
            await _signInManager.SignInAsync(user, isPersistent: true);
            
            _logger.LogInformation("User registred");

            return Ok();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SignInUserDTO signInUserDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (!await _userManager.Users.AnyAsync(user => user.UserName == signInUserDTO.UserName))
                return NotFound();

            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(signInUserDTO.UserName, signInUserDTO.Password, isPersistent: true, lockoutOnFailure: false);

            if (!signInResult.Succeeded)
                return BadRequest();

            return Ok();
        }

        [HttpGet("{UserName}")]
        [Authorize]
        public async Task<IActionResult> Profile(string UserName)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user.UserName != UserName)
                return NotFound();

            return Ok(user.Adapt<UserDTO>());
        }
    }
}
