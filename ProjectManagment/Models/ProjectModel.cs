using System.ComponentModel.DataAnnotations;

namespace ProjectManagment.Models
{
    public class ProjectModel
    {
        [Key]
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? OwnerId { get; set; }
        public UserModel? Owner { get; set; }
        public ICollection<TaskModel>? TaskModels { get; set; }
        public ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();

    }
}
