using FinancialAssistent.Infrastructure.Commands;
using FinancialAssistent.Services;
using MediatR;

namespace FinancialAssistent.Infrastructure.Handlers
{
    public class SetCostsLimitsHandler : IRequestHandler<SetCostsLimitsCommand, bool>
    {
        private readonly UserInfoService userInfoService;

        public SetCostsLimitsHandler(UserInfoService userInfoService)
        {
            this.userInfoService = userInfoService;
        }

        public Task<bool> Handle(SetCostsLimitsCommand request, CancellationToken cancellationToken)
        {
            var user = userInfoService.GetUser();
            bool result;
            if (user == null)
            {
                result = false;
                return Task.FromResult(result);
            }

            user.UserInfo.WeeklyCostsLimits = request.Model.WeeklyLimit;
            user.UserInfo.DailyCostsLimits = request.Model.DailyLimit;
            result = true;
            return Task.FromResult(result);
        }
    }
}
