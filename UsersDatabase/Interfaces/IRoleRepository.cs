using Microsoft.EntityFrameworkCore;
using UsersDatabase.Models;

namespace UsersDatabase.Interfaces
{
    public interface IRoleRepository
    {
        ICollection<Role> GetAllRoles();
        Role GetRole(string role);
        Role GetRole(int id);
        ICollection<Role> SortRoles();
        bool RoleExists(int id);
        public bool RoleExists(string name);

        bool CreateRole(Role role);

    }
}
