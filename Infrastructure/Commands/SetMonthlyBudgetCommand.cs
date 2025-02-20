using FinancialAssistent.Models;
using MediatR;

namespace FinancialAssistent.Infrastructure.Commands
{
    public class SetMonthlyBudgetCommand : IRequest<IResult>
    {
        public MonthlyBudgetModel Model;

        public SetMonthlyBudgetCommand(MonthlyBudgetModel model)
        {
            Model = model;
        }
    }
}
