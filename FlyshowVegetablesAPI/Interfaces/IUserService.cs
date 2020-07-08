using Infrastructure.Entities;
using System.Collections.Generic;

namespace FlyshowVegetablesAPI.Interfaces
{
    public interface IUserService
    {
        List<User> GetUser();

        bool IsAccountExists(string account);

        bool UpdateUser(User user);

        bool DeleteUser(int userID);

        int CreateUser(User model);
    }
}
