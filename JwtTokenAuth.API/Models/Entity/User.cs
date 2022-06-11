namespace JwtTokenAuth.API.Models.Entity
{
    public class User
    {
        public string Login { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string Role { get; set; } = "User";
    }
}