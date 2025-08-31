using Microsoft.AspNetCore.Mvc;
using StocksApp.Application.Dtos;
using StocksApp.Application.UseCases;
using StocksApp.Domain.ValueObjects;

namespace StocksApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteController : ControllerBase
    {
        [HttpGet("Quote/{symbol}")]
		public async Task<ActionResult<Quote>> GetQuote(
	[FromServices] GetQuoteBySymbolHandler getQuoteBySymbolHandler,
	[FromRoute] string symbol)
		{
            if (string.IsNullOrWhiteSpace(symbol))
            {
                return BadRequest("Key is null");
            }
            var quote = await getQuoteBySymbolHandler.GetQuoteInfoAsync(symbol);
            return Ok(quote);
        }

		[HttpGet("Price/{symbol}")]
		public async Task<ActionResult<string>> GetPrice(
	[FromServices] GetPriceHandler getPriceHandler,
	[FromRoute] string symbol)
		{
            if (string.IsNullOrWhiteSpace(symbol))
            {
                return BadRequest("Key cannot be null or empty");
            }
            var price = await getPriceHandler.GetPriceAsync(symbol);
            return Ok(price);
        }
        [HttpPut("ChangeCurrency")]
        public async Task<IActionResult> ChangeCurrency([FromServices] ChangeCurrencyHandler changeCurrencyHandler, [FromBody] ChangeCurrencyDto changeCurrencyDto)
        {
            if (string.IsNullOrWhiteSpace(changeCurrencyDto.Symbol) || string.IsNullOrWhiteSpace(changeCurrencyDto.Currency))
            {
                return BadRequest("Key or currency cannot be null or whitespace");
            }
            await changeCurrencyHandler.ChangeCurrencyAsync(changeCurrencyDto.Symbol, changeCurrencyDto.Currency);
            return NoContent();
        }
    }
}
