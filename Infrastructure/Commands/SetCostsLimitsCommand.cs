using FinancialAssistent.Models;
using MediatR;

namespace FinancialAssistent.Infrastructure.Commands
{
    public class SetCostsLimitsCommand : IRequest<bool>
    {
        public CostsLimitsModel Model { get; }

        public SetCostsLimitsCommand(CostsLimitsModel model)
        {
            Model = model;
        }
    }
}
