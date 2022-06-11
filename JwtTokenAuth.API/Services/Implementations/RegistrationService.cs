using JwtTokenAuth.API.Models.Dto;
using JwtTokenAuth.API.Models.Entity;
using JwtTokenAuth.API.Repository.Interfaces;
using JwtTokenAuth.API.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace JwtTokenAuth.API.Services.Implementations
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public RegistrationService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public void Register(UserDto userDto)
        {
            CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User();
            user.Login = userDto.Login;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _userRepository.Create(user);
        }

        public string Login(UserDto userDto)
        {
            var user = _userRepository.GetUserByLogin(userDto.Login);

            if (!VarifyPasswordHash(userDto.Password, user))
                throw new Exception("Password incorect");

            string token = CreateToken(user);

            return token;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    
        private bool VarifyPasswordHash(string password, User user)
        {
            using(var hmac = new HMACSHA512(user.PasswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(user.PasswordHash);
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Login)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(5),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                
            return jwt;
        }
    }
}
