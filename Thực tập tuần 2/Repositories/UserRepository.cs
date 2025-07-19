    using Microsoft.EntityFrameworkCore;
    using Thực_tập_tuần_2.Data;
    using Thực_tập_tuần_2.Models;

    namespace Thực_tập_tuần_2.Repositories
    {
        public class UserRepository : IUserRepository
        {
            private readonly DbContextApp _context;

            public UserRepository(DbContextApp context)
            {
                _context = context;
            }

            public async Task<User> CreateAsync(User user)
            {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

            public async Task<IEnumerable<User>> GetAllAsync()
            {
            return await _context.Users.ToListAsync();
        }

            public async Task<User?> GetByApplicationUserIdAsync(string applicationUserId)
            {
            return await _context.Users.FirstOrDefaultAsync(u => u.ApplicationUserId == applicationUserId);
        }

            public async Task<User?> GetByIdAsync(Guid id)
            {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

            async Task<bool> IUserRepository.DeleteAsync(Guid id)
            {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

            async Task<User> IUserRepository.UpdateAsync(User user)
            {
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null) throw new Exception("User not found");

            existingUser.FullName = user.FullName;
            existingUser.SĐT = user.SĐT;
            existingUser.ApplicationUserId = user.ApplicationUserId;

            await _context.SaveChangesAsync();
            return existingUser;
        }
        }
    }
