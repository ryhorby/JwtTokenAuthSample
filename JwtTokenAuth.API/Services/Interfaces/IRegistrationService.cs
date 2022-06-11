using JwtTokenAuth.API.Models.Dto;
using JwtTokenAuth.API.Models.Entity;

namespace JwtTokenAuth.API.Services.Interfaces
{
    public interface IRegistrationService
    {
        void Register(UserDto user);
        string Login(UserDto user);
    }
}
