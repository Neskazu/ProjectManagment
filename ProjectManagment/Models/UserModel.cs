using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagment.Models
{
    public class UserModel:IdentityUser
    {
        public ICollection<ProjectModel>? Projects { get; set; }
        public ICollection<TaskModel>? TasksModels { get; set; }
        public ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();
        public ICollection<TaskUser> TaskUsers { get; set; } = new List<TaskUser>();
    }
}
