using Microsoft.EntityFrameworkCore;
using Thực_tập_tuần_2.Data;
using Thực_tập_tuần_2.Models;

namespace Thực_tập_tuần_2.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly DbContextApp _context;
        public TaskRepository(DbContextApp context)
        {
            _context = context;
        }

        public async Task<TaskItem> CreateTaskAsync(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteTaskAsync(Guid id, Guid userId)
        {
            var task = await GetTaskByIdAsync(id);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<TaskItem?> GetTaskByIdAsync(Guid id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<List<TaskItem>> GetTasksByUserIdAsync(Guid userId)
        {
            return await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();
        }

        public async Task<bool> UpdateTaskAsync(TaskItem task)
        {
            _context.Tasks.Update(task);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
