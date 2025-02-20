using FinancialAssistent.Converters;
using FinancialAssistent.Entities;
using FinancialAssistent.Infrastructure.Commands;
using FinancialAssistent.Services;
using MediatR;

namespace FinancialAssistent.Infrastructure.Handlers
{
    public class AddStandartWidgetsHandler : IRequestHandler<AddStandartWidgetsCommand, List<Widgets>>
    {
        private readonly Database dbContext;
        private readonly UserInfoService userInfoService;
        private readonly IMediator mediator;

        public AddStandartWidgetsHandler(Database dbContext, UserInfoService userInfoService, IMediator mediator)
        {
            this.dbContext = dbContext;
            this.userInfoService = userInfoService;
            this.mediator = mediator;
        }

        public async Task<List<Widgets>> Handle(AddStandartWidgetsCommand request, CancellationToken cancellationToken)
        {
            var categoryExpensesResult = await mediator.Send(new GetCategoryExpencesCommand(), cancellationToken);
            var categoryExpenses = categoryExpensesResult.Labels.
                Zip(categoryExpensesResult.Values, (key, value) 
                => new { key, value })                                                      
               .ToDictionary(x => (string)x.key, x => x.value);


            var user = userInfoService.GetUser();

            List<Widgets> widgets = CreateDefaultWidgets(user.UserInfo.Id, categoryExpenses);

            await dbContext.Widgets.AddRangeAsync(widgets);
            await dbContext.SaveChangesAsync();
            return widgets;
        }

        private List<Widgets> CreateDefaultWidgets(int userInfoId, Dictionary<string, decimal> categoryExpenses)
        {
            return new List<Widgets>
            {
            new Widgets { Name = "Mandatory Expenses", IconID = 1, UserInfoId = userInfoId, Budget = 0, Expenses = categoryExpenses.GetValueOrDefault("Mandatory Expenses", 0) },
            new Widgets { Name = "Food", IconID = 9, UserInfoId = userInfoId, Budget = 0, Expenses = categoryExpenses.GetValueOrDefault("Food", 0) },
            new Widgets { Name = "Sport and Health", IconID = 33, UserInfoId = userInfoId, Budget = 0, Expenses = categoryExpenses.GetValueOrDefault("Sport and Health", 0) },
            new Widgets { Name = "Transportation", IconID = 32, UserInfoId = userInfoId, Budget = 0, Expenses = categoryExpenses.GetValueOrDefault("Transportation", 0) },
            new Widgets { Name = "Entertainment", IconID = 31, UserInfoId = userInfoId, Budget = 0, Expenses = categoryExpenses.GetValueOrDefault("Entertainment", 0) }
            };
        }
    }
}
