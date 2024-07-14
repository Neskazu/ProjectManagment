using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagment.Models
{
    public class UserModel:IdentityUser
    {
        public ICollection<ProjectModel>? Projects { get; set; }
        public ICollection<TaskModel>? TasksModels { get; set; }
    }
}
