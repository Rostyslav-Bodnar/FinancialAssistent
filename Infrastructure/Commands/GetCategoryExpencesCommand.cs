using FinancialAssistent.Transfers;
using MediatR;

namespace FinancialAssistent.Infrastructure.Commands
{
    public class GetCategoryExpencesCommand : IRequest<CategoryExpencesResult>
    {
    }
}
