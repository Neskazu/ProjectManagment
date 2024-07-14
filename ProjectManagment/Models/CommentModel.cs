﻿namespace ProjectManagment.Models
{
    public class CommentModel
    {
        public required int Id { get; set; }
        public required string Content { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required int TaskId { get; set; }
        public  required TaskModel TaskModels { get; set; }
        public  required string UserId { get; set; }
        public required UserModel User { get; set; }
    }
}