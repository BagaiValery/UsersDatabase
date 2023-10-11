using UsersDatabase.Models;

namespace UsersDatabase.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetAllUsers();
        User GetUser(int id);
        bool UserExists(int id);

        ICollection<User> FilterAge(int userAge);
        ICollection<User> FilterName(string userName);
        ICollection<User> FilterEmail(string userEmail);
        ICollection<User> FilterRole(string role);

        ICollection<User> SortByAge();
        ICollection<User> SortByName();
        ICollection<User> SortByEmail();
        ICollection<User> SortByRole();

        bool CreateUser(int roleId, User user);

    }
}
