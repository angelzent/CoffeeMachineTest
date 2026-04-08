using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMachineApi.Tests
{
    public class FakeWeatherService : WeatherMapService
    {
        private readonly double? _temp;

        public FakeWeatherService(double? temp)
            : base(new HttpClient(), new ConfigurationBuilder().Build())
        {
            _temp = temp;
        }

        public override Task<double?> GetTemperatureAsync()
        {
            return Task.FromResult(_temp);
        }
    }
}
