namespace Thực_tập_tuần_2.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
