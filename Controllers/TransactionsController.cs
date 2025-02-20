using Microsoft.AspNetCore.Mvc;
using FinancialAssistent.Services;

namespace FinancialAssistent.Controllers
{
    [ApiController]
    [Route("api/Transactions")]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionService transactionService;

        public TransactionsController(TransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        [HttpGet("transactionList")]
        public IActionResult GetTransactionsForPeriod([FromQuery] string period)
        {
            try
            {
                var transactions = transactionService.GetTransactionsForPeriod(period);
                return Ok(transactions);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal Server Error", details = ex.Message });
            }
        }
    }
}
