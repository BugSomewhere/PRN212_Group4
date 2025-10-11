using Login_Register.DAL.Entities;
using Login_Register.DAL.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login_Register.BLL.Services
{
    public class UserService
    {
        private readonly UserRepo _userRepo = new();

        public User? Login(string email, string password)
        {
            return _userRepo.Login(email, password);
        }

        public bool Register(User user)
        {
            return _userRepo.Register(user);
        }
    }
}
