using Thực_tập_tuần_2.Models;

namespace Thực_tập_tuần_2.Services
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user);
    }
}
