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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=ProjectManagement;Username=postgres;Password=qaz741");
        }
    }
}
