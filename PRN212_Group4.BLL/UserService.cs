using PRN212_Group4.DAL;
using PRN212_Group4.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN212_Group4.BLL
{
    public class UserService
    {
        private PrnGroupProjectContext repo = new();
        public User? Login(string email, string password)
        {
            return repo.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public List<User> GetAllUsers()
        {
            return repo.Users.ToList();
        }
    }
}
