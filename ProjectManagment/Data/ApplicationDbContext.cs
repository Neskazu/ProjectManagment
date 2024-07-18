using ProjectManagment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ProjectManagment.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserModel>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}
        public DbSet<ProjectModel> Projects { get; set; }
        public DbSet<TaskModel> TaskModels { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
        public DbSet<TaskUser> TaskUsers { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProjectUser>()
                .HasKey(pu => new { pu.ProjectId, pu.UserId });

            modelBuilder.Entity<ProjectUser>()
                .HasOne(pu => pu.Project)
                .WithMany(p => p.ProjectUsers)
                .HasForeignKey(pu => pu.ProjectId);

            modelBuilder.Entity<ProjectUser>()
                .HasOne(pu => pu.User)
                .WithMany(u => u.ProjectUsers)
                .HasForeignKey(pu => pu.UserId);

            modelBuilder.Entity<TaskUser>()
                .HasKey(tu => new { tu.TaskId, tu.UserId });

            modelBuilder.Entity<TaskUser>()
                .HasOne(tu => tu.Task)
                .WithMany(t => t.TaskUsers)
                .HasForeignKey(tu => tu.TaskId);

            modelBuilder.Entity<TaskUser>()
                .HasOne(tu => tu.User)
                .WithMany(u => u.TaskUsers)
                .HasForeignKey(tu => tu.UserId);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=ProjectManagement;Username=postgres;Password=qaz741");
        }
    }
}
