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
            return _context.Users.Where(p => p.Id == id).FirstOrDefault();
        }

        public bool UserExists(int id)
        {
           return _context.Users.Any(p => p.Id == id);
        }

       
        #endregion

        #region Filters
        public ICollection<User> FilterAge(int userAge)
        {
           return _context.Users.Where(p => p.Age == userAge).ToList();
        }

        public ICollection<User> FilterName(string userName)
        {
            return _context.Users.Where(p => p.Name == userName).ToList();
        }

        public ICollection<User> FilterEmail(string userEmail)
        {
            return _context.Users.Where(p => p.Email == userEmail).ToList();
        }

        public ICollection<User> FilterRole(Role role)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Sorts
        public ICollection<User> SortByAge()
        {
            return _context.Users.OrderBy(p => p.Age).ToList();
        }

        public ICollection<User> SortByName()
        {
            return _context.Users.OrderBy(p => p.Name).ToList();
        }

        public ICollection<User> SortByEmail()
        {
            return _context.Users.OrderBy(p => p.Email).ToList();
        }

        public ICollection<User> SortByRole()
        {
            return _context.Users.OrderBy(p => p.UserRole).ToList();
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
