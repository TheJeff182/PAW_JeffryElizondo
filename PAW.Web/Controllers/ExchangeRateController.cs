using Microsoft.AspNetCore.Mvc;
using PAW.Web.Services;

namespace PAW.Web.Controllers;

public class ExchangeRateController : Controller
{
    private readonly IExchangeRateService _exchangeRateService;
    private readonly ILogger<ExchangeRateController> _logger;

    public ExchangeRateController(IExchangeRateService exchangeRateService, ILogger<ExchangeRateController> logger)
    {
        _exchangeRateService = exchangeRateService;
        _logger = logger;
    }

    public async Task<IActionResult> GetRates()
    {
        try
        {
            var rates = await _exchangeRateService.GetExchangeRateAsync();
            return Ok(rates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching exchange rates");
            return StatusCode(500, new { error = "Unable to fetch exchange rates" });
        }
    }

    public IActionResult Widget()
    {
        return PartialView("_ExchangeRateWidget");
    }
}
