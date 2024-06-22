namespace ProjectManagment.Models
{
    public class UserModel
    {
        public required int Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public ICollection<ProjectModel>? Projects { get; set; }
        public ICollection<TaskModel>? Tasks { get; set; }
    }
}
