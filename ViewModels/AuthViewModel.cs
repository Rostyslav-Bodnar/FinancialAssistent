using FinancialAssistent.Models;

namespace FinancialAssistent.ViewModels
{
    public class AuthViewModel
    {
        public RegisterModel registerModel { get; set; } = new RegisterModel();
        public LoginModel loginModel { get; set; } = new LoginModel();
    }
}
