using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Thực_tập_tuần_2.Models;
using Thực_tập_tuần_2.Repositories;

namespace Thực_tập_tuần_2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _repo;

        public UserController(UserManager<ApplicationUser> userManager, IUserRepository repo)
        {
            _userManager = userManager;
            _repo = repo;
        }

        // ✅ Admin xem tất cả
        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userManager.Users.ToListAsync();

            var userList = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new
                {
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.FullName,
                    Roles = roles
                });
            }

            return Ok(userList);
        }

        // ✅ User và Admin đều dùng => logic kiểm tra trong code
        [Authorize(Roles = "User,Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var currentUserId = GetCurrentUserId(); // string
            var currentUser = await _repo.GetByApplicationUserIdAsync(currentUserId);

            // Nếu là Admin thì cho phép xem tất cả
            if (User.IsInRole("Admin"))
            {
                var user = await _repo.GetByIdAsync(id);
                if (user == null) return NotFound();
                return Ok(user);
            }

            // Nếu là User thì chỉ xem chính mình
            if (currentUser == null || currentUser.Id != id)
            {
                return Forbid(); // Không đúng user => 403
            }

            return Ok(currentUser);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            user.ApplicationUserId = GetCurrentUserId();
            var newUser = await _repo.CreateAsync(user);
            return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, User user)
        {
            var currentAppUserId = GetCurrentUserId();
            var currentUser = await _repo.GetByApplicationUserIdAsync(currentAppUserId);

            // Admin có thể cập nhật bất kỳ ai
            if (User.IsInRole("Admin"))
            {
                await _repo.UpdateAsync(user);
                return Ok();
            }

            // User chỉ được cập nhật chính mình
            if (currentUser == null || currentUser.Id != id || id != user.Id)
            {
                return Forbid();
            }

            await _repo.UpdateAsync(user);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }

        private string GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        [Authorize(policy: "AdminOnly")]
        [HttpGet("admin-only")]
        public IActionResult AdminPolicyEndpoint()
        {
            return Ok("Chỉ Admin có policy mới vào được");
        }
    }
}
