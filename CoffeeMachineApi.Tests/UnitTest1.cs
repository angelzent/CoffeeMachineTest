using CoffeeMachineApi.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeMachineApi.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Should_Return_503_On_Every_5th_Call()
        {
            var weather = new FakeWeatherService(25);
            var controller = new CoffeeMachineController(weather);
            controller.ResetCounter();

            IActionResult? result = null;
          
            for (int i = 0; i < 5; i++)
            {
                result = await controller.BrewCoffee();
            }

            var status = result as StatusCodeResult;
  
            status!.StatusCode.Should().Be(503);
        }

        [Fact]
        public async Task Should_Return_418_On_April_1()
        {
            var weather = new FakeWeatherService(25);
            var controller = new CoffeeMachineController(weather);
            controller.Clock = () => new DateTimeOffset(2026, 4, 1, 10, 0, 0, TimeSpan.Zero);

            var result = await controller.BrewCoffee();

            var status = result as StatusCodeResult;

            status!.StatusCode.Should().Be(418);
        }

        [Fact]
        public async Task Should_Return_Hot_Coffee_When_Temp_30_Or_Less()
        {
            var weather = new FakeWeatherService(25);
            var controller = new CoffeeMachineController(weather);

            var result = await controller.BrewCoffee();

            var ok = result as OkObjectResult;

            ok.Should().NotBeNull();

            ok!.Value.ToString()
                .Should()
                .Contain("Your piping hot coffee is ready");
        }

        [Fact]
        public async Task Should_Return_Iced_Coffee_When_Temp_Above_30()
        {
            var weather = new FakeWeatherService(35);
            var controller = new CoffeeMachineController(weather);

            var result = await controller.BrewCoffee();

            var ok = result as OkObjectResult;

            ok!.Value.ToString()
                .Should()
                .Contain("Your refreshing iced coffee is ready");
        }

        [Fact]
        public async Task Should_Return_Hot_Coffee()
        {
            var weather = new FakeWeatherService(25);
            var controller = new CoffeeMachineController(weather);

            var result = await controller.BrewCoffee();

            var ok = result as OkObjectResult;

            ok.Should().NotBeNull();

            ok!.Value.ToString()
                .Should()
                .Contain("Your piping hot coffee is ready");
        }

    }
}