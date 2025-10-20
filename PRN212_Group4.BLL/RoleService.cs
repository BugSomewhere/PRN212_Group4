using PRN212_Group4.DAL;
using PRN212_Group4.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN212_Group4.BLL
{
    public class RoleService
    {
        private PrnGroupProjectContext repo = new();

        public List<Role> GetAll()
        {
            return repo.Roles.ToList();
        }
    }
}
