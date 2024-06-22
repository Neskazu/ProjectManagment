using ProjectManagment.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagment.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<ProjectModel> Projects { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("ConnectionStringHere");
        }
    }
}
