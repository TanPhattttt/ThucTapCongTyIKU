namespace Thực_tập_tuần_2.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string SĐT { get; set; }
        public string ApplicationUserId { get; set; } 
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

    }
}
