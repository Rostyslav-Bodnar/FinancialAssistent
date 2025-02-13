using Microsoft.AspNetCore.Identity;

namespace FinancialAssistent.Entities
{
    public class User : IdentityUser
    {
        public string? Token { get; set; }
    }
}
