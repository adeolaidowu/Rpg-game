using Microsoft.AspNetCore.Mvc;
using Rpg_game.Dtos;
using Rpg_game.Models;
using Rpg_game.Services;
using System.Threading.Tasks;

namespace Rpg_game.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto user)
        {
            Response<int> response = await _authRepository.Register(new User { Username = user.Username }, user.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
            
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto user)
        {
            var response = await _authRepository.Login(user.Username, user.Password);
            if (response.Success)
            {
                return Ok(response);
            }
            return Unauthorized(response);
        }
    }
}
