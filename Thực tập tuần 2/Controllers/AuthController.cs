using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Thực_tập_tuần_2.Models;
using Thực_tập_tuần_2.Services;

namespace Thực_tập_tuần_2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterLoginDTO dto)
        {
            var existingUser = await _userManager.FindByNameAsync(dto.UserName);
            if (existingUser != null)
            {
                return BadRequest("Người dùng đã tồn tại");
            }
            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = $"{dto.UserName}@example.com", // có thể yêu cầu nhập email nếu cần
                FullName = dto.FullName,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, "USER");

            return Ok("Đăng ký người dùng thành công");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] RegisterLoginDTO dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);

            if (user == null)
                return Unauthorized("Invalid username or password");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!result.Succeeded)
                return Unauthorized("Invalid username or password");

            var token = _tokenService.CreateToken(user);

            return Ok(new { token });
        }
    }
}
