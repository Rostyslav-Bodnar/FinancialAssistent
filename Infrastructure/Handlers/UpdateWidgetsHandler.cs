using FinancialAssistent.Entities;
using FinancialAssistent.Infrastructure.Commands;
using FinancialAssistent.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistent.Infrastructure.Handlers
{
    public class UpdateWidgetsHandler : IRequestHandler<UpdateWidgetsCommand, IResult>
    {
        private readonly IMediator mediator;
        private readonly UserInfoService userInfoService;
        private readonly Database dbContext;

        public UpdateWidgetsHandler(UserInfoService userInfoService, Database dbContext, IMediator mediator)
        {
            this.userInfoService = userInfoService;
            this.dbContext = dbContext;
            this.mediator = mediator;
        }
        public async Task<IResult> Handle(UpdateWidgetsCommand request, CancellationToken cancellationToken)
        {
            if (request.Widgets == null || !request.Widgets.Any())
            {
                return Results.BadRequest("Invalid widget data.");
            }

            var categoryExpensesResult = await mediator.Send(new GetCategoryExpencesCommand(), cancellationToken);
            var categoryExpenses = categoryExpensesResult.Labels.Zip(categoryExpensesResult.Values, (key, value) => new { key, value })
                                                                .ToDictionary(x => x.key, x => x.value);


            var widgets = await dbContext.Widgets
                .Where(w => w.UserInfoId == userInfoService.GetUser().UserInfo.Id)
                .Where(w => request.Widgets.Select(m => m.Name).Contains(w.Name))
                .ToListAsync(cancellationToken);

            foreach (var widgetModel in request.Widgets)
            {
                var widget = widgets.FirstOrDefault(w => w.Name == widgetModel.Name);
                if (widget == null)
                {
                    return Results.NotFound($"Widget with name '{widgetModel.Name}' not found.");
                }

                widget.Budget = widgetModel.Budget;
                widget.Expenses = categoryExpenses.GetValueOrDefault(widget.Name, 0);
            }

            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);
                return Results.Ok(new { message = "Widgets updated successfully" });
            }
            catch
            {
                return Results.StatusCode(500);
            }
        }
    }
}
