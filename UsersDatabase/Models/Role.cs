using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace UsersDatabase.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public ICollection<User> Users { get; set; } = new List<User>();

        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public ICollection<UserRole> UserRole { get; set; } = new List<UserRole>();
    }
}