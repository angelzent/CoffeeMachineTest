using System.Diagnostics;
using System.Text.Json;

namespace CoffeeMachineApi
{
    public class WeatherMapService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public WeatherMapService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public virtual async Task<double?> GetTemperatureAsync()
        {
            var apiKey = _config["Weather:ApiKey"];
            var city = _config["Weather:City"];
            var units = _config["Weather:Units"];

            var url =
                $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units={units}";

            var response = await _httpClient.GetAsync(url);
            Trace.WriteLine($"vpb Weather API response status: {response}");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);

            var temp = doc.RootElement
                          .GetProperty("main")
                          .GetProperty("temp")
                          .GetDouble();

            return temp;
        }

    }
}
