using Microsoft.EntityFrameworkCore;
using System.Data;
using UsersDatabase.Data;
using UsersDatabase.Interfaces;
using UsersDatabase.Models;

namespace UsersDatabase.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly UserContext _context;

        public RoleRepository (UserContext userContext)
        {
            _context = userContext;
        }

        public bool CreateRole(Role role)
        {
            _context.Roles.Add(role);

            return(_context.SaveChanges() >0);
        }

        public ICollection<Role> GetAllRoles()
        {
            return _context.Roles.OrderBy(p => p.Id).ToList();
        }

        public Role GetRole(int id)
        {
           return _context.Roles.Where(p => p.Id == id).FirstOrDefault();
        }

        public Role GetRole(string role)
        {
            return _context.Roles.Where(p => p.RoleName == role).FirstOrDefault();
        }

        public bool RoleExists(int id)
        {
            return _context.Roles.Any(p => p.Id == id);
        }
        public bool RoleExists(string name)
        {
            return _context.Roles.Any(p => p.RoleName == name);
        }

        public ICollection<Role> SortRoles()
        {
            return _context.Roles.OrderBy(p => p.RoleName).ToList();
        }
    }
}
