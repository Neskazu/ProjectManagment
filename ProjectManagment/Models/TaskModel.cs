using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjectManagment.Models
{
    public class TaskModel
    {
        [Key]
        public required int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Deadline { get; set; }
        public required TaskStatus Status { get; set; }
        public required int ProjectId { get; set; }
        public required ProjectModel Project { get; set; }
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
