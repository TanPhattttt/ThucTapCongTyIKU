using Thực_tập_tuần_2.Models;

namespace Thực_tập_tuần_2.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(Guid id);
        Task<User?> GetByApplicationUserIdAsync(string applicationUserId);
    }
}
