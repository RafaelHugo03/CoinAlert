using CoinAlertApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoinAlertApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CryptoController(ICryptoPriceService cryptoPriceService) : ControllerBase
{
    [HttpPost("prices/fetch")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    public async Task<IActionResult> FetchPrices()
    {
        var prices = await cryptoPriceService.GetAllAndTransmitAsync();

        if (prices is null)
            return StatusCode(StatusCodes.Status502BadGateway, new { error = "Failed to fetch prices from CoinGecko" });

        return Ok(prices);
    }
}
