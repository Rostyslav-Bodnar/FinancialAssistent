using Microsoft.AspNetCore.Identity;

namespace FinancialAssistent.Entities
{
    public class User : IdentityUser
    {
        public decimal Cash {  get; set; }
    }
}
