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
            modelBuilder.HasDefaultSchema("users");

            modelBuilder.Entity<Role>(b =>
            {
                //b.Property(a => a.Id).ValueGeneratedNever();
                b.HasKey(a => a.Id);
                b.HasIndex(b => b.RoleName).IsUnique();
            });

            modelBuilder.Entity<User>(b =>
            {
                // Многие ко многим
                b
                 .HasMany(p => p.Roles)
                 .WithMany(a => a.Users)
                 .UsingEntity<UserRole>(
                    role => role
                        .HasOne(pa => pa.Role)
                        .WithMany(a => a.UserRole)
                        .HasForeignKey(pa => pa.RoleId),
                    user => user
                        .HasOne(pa => pa.User)
                        .WithMany(p => p.UserRole)
                        .HasForeignKey(pa => pa.UserId));

                //b.HasData(Users);
            });

            //modelBuilder.Entity<UserRole>()
            //    .HasKey(e => new { e.UserId, e.RoleId });
            //modelBuilder.Entity<UserRole>()
            //    .HasOne(e => e.Role)
            //    .WithMany(e => e.UserRole)
            //    .HasForeignKey(e => e.RoleId)
            //    .IsRequired();
            //modelBuilder.Entity<UserRole>()
            //    .HasOne(e => e.User)
            //    .WithMany(e => e.UserRole)
            //    .HasForeignKey(e => e.UserId)
            //    .IsRequired();
        }

    }
}