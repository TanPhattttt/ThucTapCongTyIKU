namespace Thực_tập_tuần_2.Repositories
{
    public class CreateOrUpdateTaskDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public Guid UserId { get; set; }
    }
}
