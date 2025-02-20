using FinancialAssistent.Models;
using MediatR;

namespace FinancialAssistent.Infrastructure.Commands
{
    public class UpdateWidgetsCommand : IRequest<IResult>
    {
        public WidgetUpdateModel[] Widgets { get; }

        public UpdateWidgetsCommand(WidgetUpdateModel[] widgets)
        {
            Widgets = widgets;
        }
    }
}
