using FinancialAssistent.Entities;
using MediatR;

namespace FinancialAssistent.Infrastructure.Commands
{
    public class AddStandartWidgetsCommand : IRequest<List<Widgets>>
    {
    }
}
