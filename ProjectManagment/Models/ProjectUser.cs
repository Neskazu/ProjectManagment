namespace ProjectManagment.Models
{
    public enum ProjectRole
    {
        Owner,
        Editor,
        Viewer
    }
    public class ProjectUser
    {
        public int ProjectId {  get; set; }
        public ProjectModel? Project { get; set; }
        public string? UserId { get; set; }
        public UserModel? User { get; set; }
        public ProjectRole Role { get; set; }

    }
}
