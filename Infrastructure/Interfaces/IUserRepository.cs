using Infrastructure.Entities;
using System.Linq;


namespace Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        IQueryable<User> GetAll();

        bool UpdateUser(User user);

        bool DeleteUser(int userID);

        int CreateUser(User model);
    }
}
