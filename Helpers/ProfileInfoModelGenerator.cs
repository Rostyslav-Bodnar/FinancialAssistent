using FinancialAssistent.Services;
using FinancialAssistent.ViewModels;

namespace FinancialAssistent.Helpers
{
    public class ProfileInfoModelGenerator
    {
        private readonly UserInfoService userInfoService;
        private readonly BankCardService bankCardService;

        public ProfileInfoModelGenerator(UserInfoService userInfoService, 
            BankCardService bankCardService)
        {
            this.userInfoService = userInfoService;
            this.bankCardService = bankCardService;
        }

        public async Task<ProfileInfoViewModel?> GenerateModel()
        {
            var user = userInfoService.GetUser();
            if (user == null) return null;

            return new ProfileInfoViewModel
            {
                User = user,
                UserInfo = user.UserInfo,
                bankCardEntities = await bankCardService.GetBankCards(user.Id)
            };
        }
    }
}
