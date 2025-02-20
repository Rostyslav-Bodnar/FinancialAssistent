using FinancialAssistent.Infrastructure.Commands;
using FinancialAssistent.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAssistent.Services
{
    [Route("api/MonthlyBudget")]
    [ApiController]
    public class MonthlyBudgetService
    {
        private readonly IMediator mediator;

        public MonthlyBudgetService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("budget")]
        public async Task<MonthlyBudgetModel> GetMonthlyBudgetInfo()
        {
            var result = await mediator.Send(new GetMonthlyBudgetCommand());
            return result;
        }

        [HttpPost("setBudget")]
        public async Task<IResult> SetMonthlyBudget([FromBody] MonthlyBudgetModel model)
        {
            if(model == null)
            {
                return Results.BadRequest("Invalid budget data.");
            }

            return await mediator.Send(new SetMonthlyBudgetCommand(model));
        }


    }

}
