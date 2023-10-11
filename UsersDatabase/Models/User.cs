using System.Text.Json.Serialization;

namespace UsersDatabase.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public ICollection<Role> Roles { get; set; } = new List<Role>();

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public ICollection<UserRole> UserRole { get; set; } = new List<UserRole>();

    }
}
