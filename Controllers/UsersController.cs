using Microsoft.AspNetCore.Mvc;
using zaebal.Application.Interfaces;
using zaebal.Application.DTO;

namespace zaebal.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
        {
            var user = await _userService.RegisterUserAsync(request);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserAsync(id);
            if (user == null)
                return NotFound("Ошибка 404. Пользователь не найден.");

            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10,
            [FromQuery] string email = null, [FromQuery] string phone = null)
        {
            var users = await _userService.GetUsersAsync(page, pageSize, email, phone);
            return Ok(users);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
                return NotFound("Пользователь не найден.");

            return NoContent();
        }
    }
}
