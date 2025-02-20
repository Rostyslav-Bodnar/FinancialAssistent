using FinancialAssistent.Infrastructure.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAssistent.Services
{
    [Route("api/BudgetForecast")]
    [ApiController]
    public class BudgetForecastService
    {
        private readonly IMediator mediator;

        public BudgetForecastService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{type}")]
        public async Task<IResult> Predict(string type)
        {
            try
            {
                var result = await mediator.Send(new PredictBudgetForecastCommand(type));
                return Results.Ok(result);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }
    }
}
