using JwtTokenAuth.API.Models.Dto;
using JwtTokenAuth.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JwtTokenAuth.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IRegistrationService _registrationService;

        public AuthController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost("register")]
        public IActionResult Register(UserDto user)
        {
            _registrationService.Register(user);
            return Ok("Operation succesfull");
        }

        [HttpPost("login")]
        public ActionResult<string> Login(UserDto user)
        {
            return Ok(_registrationService.Login(user));
        }
    }
}
