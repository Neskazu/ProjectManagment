namespace ProjectManagment.Models
{
    public class ProjectModel
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required int OwnerId { get; set; }
        public required UserModel Owner { get; set; }
        public ICollection<TaskModel>? Tasks { get; set; }

    }
}
