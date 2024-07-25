namespace ProjectManagment.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TaskModelId { get; set; }
        public  string UserId { get; set; }
        public TaskModel TaskModels { get; set; }
        public UserModel User { get; set; }
    }
}