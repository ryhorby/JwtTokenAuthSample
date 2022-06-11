using JwtTokenAuth.API.Models.Entity;

namespace JwtTokenAuth.API.Repository.Interfaces
{
    public interface IUserRepository
    {
        void Create(User entity);

        User GetUserByLogin(string login);
    }
}
