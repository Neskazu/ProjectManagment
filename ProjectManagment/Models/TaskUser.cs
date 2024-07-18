namespace ProjectManagment.Models
{
    public class TaskUser
    {
        public int TaskId { get; set; }
        public TaskModel? Task { get; set; }

        public string? UserId { get; set; }
        public UserModel? User { get; set; }
    }
}
