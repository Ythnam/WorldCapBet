using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldCapBet.ApplicationException;
using WorldCapBet.Data;
using WorldCapBet.Helpers;
using WorldCapBet.Model;

namespace WorldCapBet.BLL
{
    public class UserService : IUserService
    {
        private WorldCapBetContext context;

        public UserService(WorldCapBetContext _context)
        {
            this.context = _context;
        }

        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = context.User.SingleOrDefault(x => x.Username == username);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPassword(password, user.Password))
                return null;

            // authentication successful
            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return context.User;
        }

        public User GetById(int id)
        {
            return context.User.Find(id);
        }

        public User Create(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (context.User.Any(x => x.Username == user.Username))
                throw new AppException("Username " + user.Username + " is already taken");

            if (context.User.Any(x => x.Email == user.Email))
                throw new AppException("Email adress " + user.Email + " is already used");
           
            user.Password = CryptoHelper.Encrypt(password);

            context.User.Add(user);
            context.SaveChanges();  // needed to have the ID of current User object on the following code

            // Add pronostic for each match already created
            foreach(Match match in context.Match)
            {
                context.Pronostic.Add(new Pronostic
                {
                    IdUser = user.Id,
                    IdMatch = match.Id
                });
            }
            context.SaveChanges();

            return user;
        }

        public void UpdateProfile(User userParam, string password = null)
        {
            var user = context.User.Find(userParam.Id);

            if (user == null)
                throw new AppException("User not found");

            if (userParam.Username != user.Username)
            {
                // username has changed so check if the new username is already taken
                if (context.User.Any(x => x.Username == userParam.Username))
                    throw new AppException("Username " + userParam.Username + " is already taken");
            }

            // update user properties
            user.FirstName = userParam.FirstName;
            user.LastName = userParam.LastName;
            user.Username = userParam.Username;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                user.Password = CryptoHelper.Encrypt(password);
            }

            context.User.Update(user);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = context.User.Find(id);
            if (user != null)
            {
                context.User.Remove(user);
                var pronostics = context.Pronostic.Where(p => p.IdUser == user.Id);
                foreach (Pronostic p in pronostics)
                    context.Remove(p);
                context.SaveChanges();
            }
        }


        private bool VerifyPassword(string password, string storedPassword)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            // Crypt the password sent by client
            string encryptedPassword = CryptoHelper.Encrypt(password);

            // Compare password sent by client with the one on Database
            for(int i = 0; i < encryptedPassword.Length; i++)
            {
                if (encryptedPassword[i] != storedPassword[i]) return false;
            }

            return true;
        }
    }
}