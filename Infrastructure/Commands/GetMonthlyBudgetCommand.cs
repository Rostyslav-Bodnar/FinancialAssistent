using FinancialAssistent.Models;
using MediatR;

namespace FinancialAssistent.Infrastructure.Commands
{
    public class GetMonthlyBudgetCommand : IRequest<MonthlyBudgetModel>
    {

    }
}
