namespace JwtTokenAuth.API.Models.Dto
{
    public class UserDto
    {
        public string Login { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = "User";
    }
}