
using FinancialAssistent.Entities;
using FinancialAssistent.Infrastructure.Commands;
using FinancialAssistent.Services;
using MediatR;

namespace FinancialAssistent.Infrastructure.Handlers
{
    public class SetMonthlyBudgetHandler : IRequestHandler<SetMonthlyBudgetCommand, IResult>
    {
        private readonly Database dbContext;
        private readonly UserInfoService userInfoService;

        public SetMonthlyBudgetHandler(Database dbContext, UserInfoService userInfoService)
        {
            this.dbContext = dbContext;
            this.userInfoService = userInfoService;
        }

        public async Task<IResult> Handle(SetMonthlyBudgetCommand request, CancellationToken cancellationToken)
        {
            if (request.Model == null)
            {
                return Results.BadRequest("Invalid budget data.");
            }

            decimal maxBudget = 10000; //balanceInfoService.GetTotalBalance();
            if (request.Model.MonthlyBudget > maxBudget)
            {
                return Results.BadRequest($"Budget cannot exceed {maxBudget}.");
            }

            var user = userInfoService.GetUser();
            if (user == null)
            {
                return Results.Unauthorized();
            }

            user.UserInfo.MonthlyBudget = request.Model.MonthlyBudget;
            await dbContext.SaveChangesAsync();

            return Results.Ok(new { message = "Monthly budget updated successfully", budget = user.UserInfo.MonthlyBudget });
        }
    }
}
