namespace FinancialAssistent.Models
{
    public class AuthViewModel
    {
        public RegisterViewModel registerModel { get; set; } = new RegisterViewModel();
        public LoginViewModel loginModel { get; set; } = new LoginViewModel();
    }
}
