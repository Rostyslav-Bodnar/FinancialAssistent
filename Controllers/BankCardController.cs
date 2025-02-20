using FinancialAssistent.Entities;
using FinancialAssistent.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinancialAssistent.Controllers
{
    [Route("api/BankCard")]
    [ApiController]
    [Authorize]
    public class BankCardController : ControllerBase
    {
        private readonly BankCardService bankCardService;

        public BankCardController(BankCardService bankCardService)
        {
            this.bankCardService = bankCardService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetCards()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var cards = await bankCardService.GetBankCards(userId);
            return Ok(cards);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddCard([FromBody] BankCardEntity card)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var success = await bankCardService.AddBankCard(userId, card);
            if (!success) return BadRequest(new { message = "Не вдалося додати картку" });

            return Ok(new { message = "Картку додано успішно", card });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteCard(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var success = await bankCardService.DeleteBankCard(userId, id);
            if (!success) return NotFound(new { message = "Картка не знайдена" });

            return Ok(new { message = "Картку видалено успішно" });
        }
    }
}
