using Microsoft.EntityFrameworkCore;
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
            return repo.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower() && u.Password.ToLower() == password.ToLower());
        }

        
           public List<User> GetAllUsers()
        {
            repo.Roles.Include(r => r.Id).ToList();
            return repo.Users.Include(u => u.RoleId).ToList();
        }

        
        public User? GetUserById(int id)
        {
            return repo.Users.FirstOrDefault(u => u.Id == id);
        }

        public Boolean Register(User user)
        {
            var existingUser = repo.Users.FirstOrDefault(u => u.Email == user.Email);
            user.TotalCredit = 100000; // Initialize TotalCredit to 0 for new users
            user.RoleId = 2; // Default role is User
            if (existingUser != null)
            {
                return false;
            }
            repo.Users.Add(user);
            repo.SaveChanges();
            return true;
        }

        public void DeleteUser(int id)
        {
            var user = repo.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                repo.Users.Remove(user);
                repo.SaveChanges();
            }
        }
        public void UpdateUser(User updatedUser)
        {
            var user = repo.Users.FirstOrDefault(u => u.Id == updatedUser.Id);
            if (user != null)
            {
                user.FullName = updatedUser.FullName;
                user.Email = updatedUser.Email;
                user.RoleId = updatedUser.RoleId;
                user.Password = updatedUser.Password;
                user.TotalCredit = updatedUser.TotalCredit;
                repo.SaveChanges();
            }
        }
    }
}
