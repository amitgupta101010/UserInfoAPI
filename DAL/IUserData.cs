using System.Collections.Generic;
using UserInfromationAPI.Models;

namespace UserInfromationAPI.DAL
{
    public interface IUserData
    {
        string AddUser(User user);
        List<User> GetAllUser(int? id);
        string DeleteUser(int id);
        string UpdateUser(int id, User user);
    }
}
