using FinancialAssistent.Infrastructure.Commands;
using FinancialAssistent.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAssistent.Services
{
    [Route("api/CategoryExpences")]
    [ApiController]
    public class CostLimitsService
    {
        private readonly IMediator _mediator;

        public CostLimitsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("getLimits")]
        public async Task<CostsLimitsModel> GetCostsLimits()
        {
            var result = await _mediator.Send(new GetCostsLimitsCommand());
            return result;
        }

        [HttpPost("setLimits")]
        public async Task<IResult> SetCostsLimits([FromBody] CostsLimitsModel model)
        {
            if (model == null)
            {
                return Results.BadRequest("Invalid budget data.");
            }

            var success = await _mediator.Send(new SetCostsLimitsCommand(model));
            return success ? Results.Ok(new { message = "Monthly budget updated successfully" })
                           : Results.Unauthorized();
        }

    }
}
