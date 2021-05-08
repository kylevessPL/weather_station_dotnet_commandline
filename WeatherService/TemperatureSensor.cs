using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WeatherService
{
    [DataContract]
    public class TemperatureSensor : Sensor, ITemperature
    {
        private const int Interval = 5;
        private double _temperature;

        public TemperatureSensor()
        {
            UpdateTemperature();
        }

        [DataMember(Name = "TemperatureUnit")]
        private string TemperatureUnitName
        {
            get => Enum.GetName(typeof(TemperatureUnit), TemperatureUnit);
            set => TemperatureUnit = (TemperatureUnit) Enum.Parse(typeof(TemperatureUnit), value, true);
        }

        [DataMember]
        public double Temperature
        {
            get => Math.Round(_temperature, 2);
            private set => _temperature = value;
        }

        public TemperatureUnit TemperatureUnit { get; set; } = TemperatureUnit.Celsius;

        private async void UpdateTemperature()
        {
            while (true)
                try
                {
                    await Task.Run(() =>
                    {
                        Temperature = GetTemperatureValue();
                        var batteryLevel = GetBatteryLevel();
                        var measurement = new Measurement
                        {
                            SensorName = Name,
                            BatteryLevel = batteryLevel,
                            DateTime = DateTime.Now,
                            Value = Temperature,
                            Type = MeasurementType.Temperature
                        };
                        MeasurementChanged(measurement);
                    });
                    await Task.Delay(TimeSpan.FromSeconds(Interval));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
        }

        private static double GetTemperatureValue()
        {
            var random = new Random();
            return random.NextDouble() * 60 - 30;
        }
    }
}