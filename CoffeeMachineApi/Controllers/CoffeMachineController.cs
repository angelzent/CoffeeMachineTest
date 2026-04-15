using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;

namespace CoffeeMachineApi.Controllers
{
    [ApiController]
    [Route("/")]
    public class CoffeeMachineController : ControllerBase
    {
        private static int _counter = 0;
        private static readonly object _lock = new();
        public Func<DateTimeOffset> Clock { get; set; } = () => DateTimeOffset.Now;
        private readonly WeatherMapService _weatherMapService;

        public CoffeeMachineController(WeatherMapService weatherMapService)
        {
            _weatherMapService = weatherMapService;
        }

        [HttpGet("brew-coffee")]
        public async Task<IActionResult> BrewCoffee()
        {
            // Rule #3 — April 1st returns 418
            var now = Clock();
            if (now.Month == 4 && now.Day == 1)
            {
                return StatusCode(418); // I'm a teapot
            }

            int currentCount;

            lock (_lock)
            {
                _counter++;
                currentCount = _counter;
            }

            // Rule #2 — every 5th call returns 503
            if (currentCount % 5 == 0)
            {
                return StatusCode(503);
            }

            // Rule #1 — normal response
            var message = "Your piping hot coffee is ready";
            var temperature = await _weatherMapService.GetTemperatureAsync();
            Trace.WriteLine($"vpb the temperature is: {(temperature.HasValue ? temperature.Value : 0)}");

            // new Rule #4 — if temperature is above 30°C, return iced coffee message
            if (temperature.HasValue && temperature.Value > 30)
            {
                message = "Your refreshing iced coffee is ready";
            }
            var response = new
            {
                message,
                prepared = now.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture)
            };

            return Ok(response);
        }

        [NonAction]
        public void ResetCounter()
        {
            lock (_lock)
            {
                _counter = 0;
            }
        }

    }
}