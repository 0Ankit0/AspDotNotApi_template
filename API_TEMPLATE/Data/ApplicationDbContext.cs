using API_TEMPLATE.Data.Users;
using Microsoft.EntityFrameworkCore;

namespace API_TEMPLATE.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<RoleMaster> RoleMasters { get; set; }
        public DbSet<UserMaster> UserMasters { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring RoleMaster
            modelBuilder.Entity<RoleMaster>()
                .HasKey(r => r.RoleId);

            modelBuilder.Entity<RoleMaster>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .OnDelete(DeleteBehavior.SetNull);  // Prevent cascading delete

            modelBuilder.Entity<RoleMaster>()
                .HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.SetNull);  // Prevent cascading delete


            // Configuring UserMaster
            modelBuilder.Entity<UserMaster>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<UserMaster>()
                .HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.SetNull);  // Prevent cascading delete

            // Configuring UserRole
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => ur.UserRoleId);

            modelBuilder.Entity<UserRole>()
                .Property(ur => ur.UserRoleId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.SetNull);  // Prevent cascading delete

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.SetNull);  // Prevent cascading delete



        }
    }


}
