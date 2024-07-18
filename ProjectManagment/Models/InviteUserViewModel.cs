namespace ProjectManagment.Models
{
    internal class InviteUserViewModel
    {
        public int ProjectId { get; set; }
        public string Email { get; set; }
        public ProjectRole Role { get; set; }
    }
}