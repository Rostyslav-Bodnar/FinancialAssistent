using Microsoft.AspNetCore.Identity;

namespace FinancialAssistent.Entities
{
    public class User : IdentityUser
    {
        public UserInfo UserInfo { get; set; }
    }
}
