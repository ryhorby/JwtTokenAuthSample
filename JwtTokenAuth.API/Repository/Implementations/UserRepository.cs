using JwtTokenAuth.API.Models.Entity;
using JwtTokenAuth.API.Repository.Interfaces;

namespace JwtTokenAuth.API.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private List<User> users = new List<User>();

        public void Create(User user)
        {
            if (users.Count == 0)
                users.Add(user);
            else
            {
                foreach (User repositoryUser in users)
                {
                    if (repositoryUser.Login == user.Login)
                        throw new Exception("User with the same login alreay exist");
                }

                users.Add(user);
            }
        }

        public User GetUserByLogin(string login)
        {
            if(users.Count == 0)
                throw new ArgumentNullException("The table user is empty");

            foreach(User repositoryUser in users)
            {
                if(repositoryUser.Login == login)
                {
                    return repositoryUser;
                }
            }

            throw new ArgumentException("The user doesn`t exist");
        }
    }
}
