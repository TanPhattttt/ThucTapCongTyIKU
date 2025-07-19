using Thực_tập_tuần_2.Models;

namespace Thực_tập_tuần_2.Repositories
{
    public interface ITaskRepository
    {
        Task<List<TaskItem>> GetTasksByUserIdAsync(Guid userId);
        Task<TaskItem?> GetTaskByIdAsync(Guid id);
        Task<TaskItem> CreateTaskAsync(TaskItem task);
        Task<bool> UpdateTaskAsync(TaskItem task);
        Task<bool> DeleteTaskAsync(Guid id, Guid userId);
    }
}
