using FinancialAssistent.Entities;
using MediatR;

namespace FinancialAssistent.Infrastructure.Commands
{
    public class GetWidgetsCommand : IRequest<List<Widgets>>
    {
    }
}
