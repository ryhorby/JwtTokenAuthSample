using JwtTokenAuth.API.Models.Dto;
using JwtTokenAuth.API.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace JwtTokenAuth.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;

        //Easiest repository
        private static User _user;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Register user into repository
        /// </summary>
        /// <param name="request">Dto object</param>
        /// <returns>User object which was added</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST auth/register
        ///     {        
        ///       "login": "login",
        ///       "password": "password"       
        ///     }
        ///  Another request if you want to add role:
        ///  
        ///     POST auth/register
        ///     {        
        ///       "login": "login",
        ///       "password": "password" 
        ///       "role": "admin"
        ///     }
        /// </remarks>
        /// <response code="201">Returns the newly created user</response>
        /// <response code="400">If you didn`t fill login or password</response>
        [HttpPost("register")]
        public IActionResult Register(UserDto request)
        {
            if (request.Login == string.Empty | request.Password == string.Empty)
                return BadRequest("Login or password is empty");

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User();
            user.Login = request.Login;
            user.Role = request.Role;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _user = user;

            return Created("/auth/register", _user);
        }

        /// <summary>
        /// Loging into your web application
        /// </summary>
        /// <param name="request">Dto object</param>
        /// <returns>User object which was added</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST auth/login
        ///     {        
        ///       "login": "login",
        ///       "password": "password"       
        ///     }
        /// </remarks>
        /// <response code="200">Returns jwt token for authentication</response>
        /// <response code="401">If login or password was incorect</response>
        /// <response code="404">If repository was empty</response>
        [HttpPost("login")]
        public ActionResult<string> Login(UserDto request)
        {
            if (_user == null)
                return NotFound("User wasn`t found");

            if (request.Login != _user.Login | !VarifyPasswordHash(request.Password, _user))
                return Unauthorized("Login or password incorect");

            string token = CreateToken(_user);

            return Ok(token);
        }


        //Service
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VarifyPasswordHash(string password, User user)
        {
            using (var hmac = new HMACSHA512(user.PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(user.PasswordHash);
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
