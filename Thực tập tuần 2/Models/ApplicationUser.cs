using Microsoft.AspNetCore.Identity;

namespace Thực_tập_tuần_2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
