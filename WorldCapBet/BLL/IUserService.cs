using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldCapBet.Model;

namespace WorldCapBet.BLL
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User Create(User user, string password);
        void UpdateProfile(User user, string password = null);
        void Delete(int id);
    }
}
