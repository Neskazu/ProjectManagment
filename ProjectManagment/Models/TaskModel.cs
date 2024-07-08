using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjectManagment.Models
{
    public class TaskModel
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        private DateTime? deadline;

        public DateTime? Deadline
        {
            get => deadline;
            set
            {
                if (value.HasValue)
                {
                    deadline = DateTime.SpecifyKind(value.Value, DateTimeKind.Utc);
                }
                else
                {
                    deadline = null;
                }
            }
        }

        public TaskStatus Status { get; set; }
        public required int ProjectId { get; set; }
        public ProjectModel? Project { get; set; }
        public int? AssignedUserId { get; set; }
        public UserModel? AssignedUser { get; set; }
        public ICollection<CommentModel> Comments { get; set; } = new List<CommentModel>();
    }
    public enum TaskStatus
    {
        New,
        InProgress,
        Completed,
        Abandoned
    }
}
