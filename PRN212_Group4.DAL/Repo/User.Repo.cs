using Login_Register.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login_Register.DAL.Repo
{
    public class UserRepo
    {
        public PrnGroupProjectContext context;

        public UserRepo()
        {
            context = new PrnGroupProjectContext();
        }

        public User? Login(string email, string password)
        {
            return context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public Boolean Register(User user)
        {
            var existingUser = context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (existingUser != null)
            {
                return false;
            }
            context.Users.Add(user);
            context.SaveChanges();
            return true;
        }

    }
}
