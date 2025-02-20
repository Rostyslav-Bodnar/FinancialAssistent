using FinancialAssistent.Infrastructure.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinancialAssistent.Services
{
    [Route("api/CategoryExpences")]
    [ApiController]
    public class CategoryExpencesService
    {
        private readonly IMediator mediator;

        public CategoryExpencesService(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpGet("category")]
        public async Task<IActionResult> GetCategoryExpences()
        {
            var result = await mediator.Send(new GetCategoryExpencesCommand());
            return new JsonResult(result);
        }
    }
}
