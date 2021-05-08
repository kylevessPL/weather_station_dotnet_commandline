using System;

namespace WeatherService
{
    public static class Program
    {
        public static void Main()
        {
            var pressureSensor = new PressureSensor {Name = "PressureS1"};
            var temperatureSensor = new TemperatureSensor
            {
                Name = "TemperatureS1",
                TemperatureUnit = TemperatureUnit.Fahrenheit
            };
            var temperatureAndHumiditySensor = new TemperatureAndHumiditySensor
                {TemperatureUnit = TemperatureUnit.Celsius};
            var station1 = new WeatherStation();
            station1.RegisterSensor(pressureSensor);
            station1.RegisterSensor(temperatureSensor);
            var station2 = new WeatherStation();
            station2.RegisterSensor(pressureSensor);
            station2.RegisterSensor(temperatureAndHumiditySensor);
            Console.ReadLine();
        }
    }
}