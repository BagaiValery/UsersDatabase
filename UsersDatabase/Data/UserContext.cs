using Microsoft.EntityFrameworkCore;
using UsersDatabase.Models;

namespace UsersDatabase.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) 
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UsersRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(e => new {e.UserId, e.RoleId});
            modelBuilder.Entity<UserRole>()
                .HasOne(e => e.Role)
                .WithMany(e => e.UserRole)
                .HasForeignKey(e => e.RoleId);
            modelBuilder.Entity<UserRole>()
                .HasOne(e => e.User)
                .WithMany(e => e.UserRole)
                .HasForeignKey(e => e.UserId);
        }

    }
}
