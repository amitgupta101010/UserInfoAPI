using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserInfromationAPI.Models
{
    public interface IUserInformation
    {
        string AddUser(User user);
        List<User> GetAllUser(int? ID);
        string DeleteUser(int ID);
        string UpdateUser(int ID, User user);
    }
}
