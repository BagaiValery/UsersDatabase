using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using UsersDatabase.Data;
using UsersDatabase.Interfaces;
using UsersDatabase.Models;

namespace UsersDatabase.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;
        public UserRepository(UserContext context) 
        { 
            _context= context;
        }

        public ICollection<User> GetAllUsers()
        {
            return _context.Users.OrderBy(p => p.Id).ToList();
        }

        #region GetOneBy...
        public User GetUser(int id)
        {
            return _context.Users.Where(p => p.Id == id).Include(r => r.UserRole).ThenInclude(r => r.Role).FirstOrDefault();
        }

        public bool UserExists(int id)
        {
           return _context.Users.Any(p => p.Id == id);
        }

       
        #endregion

        #region Filters
        public ICollection<User> FilterAge(int userAge)
        {
           return _context.Users.Where(p => p.Age == userAge).Include(r => r.UserRole).ThenInclude(r => r.Role).ToList();
        }

        public ICollection<User> FilterName(string userName)
        {
            return _context.Users.Where(p => p.Name == userName).Include(r => r.UserRole).ThenInclude(r => r.Role).ToList();
        }

        public ICollection<User> FilterEmail(string userEmail)
        {
            return _context.Users.Where(p => p.Email == userEmail).Include(r => r.UserRole).ThenInclude(r => r.Role).ToList();
        }

        public ICollection<User> FilterRole(string role)
        {
            return _context.Users.Include(
                r => r.UserRole.Where(
                    p => p.Role.RoleName.Equals(role))).Include(r => r.UserRole).ThenInclude(r => r.Role).ToList();
        }

        #endregion

        #region Sorts
        public ICollection<User> SortByAge()
        {
            return _context.Users.OrderBy(p => p.Age).Include(r => r.UserRole).ThenInclude(r => r.Role).ToList();
        }

        public ICollection<User> SortByName()
        {
            return _context.Users.OrderBy(p => p.Name).Include(r => r.UserRole).ThenInclude(r => r.Role).ToList();
        }

        public ICollection<User> SortByEmail()
        {
            return _context.Users.OrderBy(p => p.Email).Include(r => r.UserRole).ThenInclude(r => r.Role).ToList();
        }

        public ICollection<User> SortByRole()
        {
            return _context.Users.OrderBy(p => p.UserRole).Include(r => r.UserRole).ThenInclude(r => r.Role).ToList();
        }

        #endregion

        public bool CreateUser(int roleId, User user)
        {
            var userRoleEntity = _context.Roles.Where(p => p.Id == roleId).FirstOrDefault();

            var userRole = new UserRole()
            {
                Role = userRoleEntity,
                User = user,
            };

            _context.Add(userRole);
            _context.Add(user);

            return(_context.SaveChanges() > 0);

        }
    }
}
