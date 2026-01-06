using Business.Services;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Project.Shared.Model;
using Shared.Command;

namespace Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtService _jwtService;

        public AuthController(IUserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { Message = "Username and password are required." });

            try
            {
                var user = await _userService.RegisterAsync(dto.Username, dto.Password);

                return Ok(new
                {
                    user.Id,
                    user.Username,
                    user.Role
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest(new { Message = "Username and password are required." });

            User user;
            try
            {
                user = await _userService.AuthenticateAsync(dto.Username, dto.Password);
            }
            catch
            {
                return Unauthorized(new { Message = "Invalid username or password." });
            }

            var token = _jwtService.GenerateToken(user.Id.ToString(), user.Role);

            return Ok(new
            {
                Token = token,
                User = new { user.Id, user.Username, user.Role }
            });
        }
    }
}
