using FinancialAssistent.Converters;
using FinancialAssistent.Entities;
using FinancialAssistent.Infrastructure.Commands;
using FinancialAssistent.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistent.Services
{
    [Route("api/WidgetService")]
    [ApiController]
    public class WidgetService
    {
        private readonly IMediator mediator;
        public WidgetService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("getWidgets")]
        public async Task<List<Widgets>> GetWidgets()
        {
            return await mediator.Send(new GetWidgetsCommand());
        }

        [HttpPost("addStandartWidgets")]
        public async Task<List<Widgets>> AddStandartWidgets()
        {
            return await mediator.Send(new AddStandartWidgetsCommand());
        }

        [HttpPost("updateWidgets")]
        public async Task<IResult> UpdateWidgets([FromBody] WidgetUpdateModel[] model)
        {
            return await mediator.Send(new UpdateWidgetsCommand(model));

        }
    }
}
