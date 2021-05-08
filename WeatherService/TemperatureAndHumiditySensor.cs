using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WeatherService
{
    [DataContract]
    public class TemperatureAndHumiditySensor : Sensor, ITemperature, IHumidity
    {
        private const int Interval = 1;
        private double _humidity;
        private double _temperature;

        public TemperatureAndHumiditySensor()
        {
            UpdateTemperature();
            UpdateHumidity();
        }

        [DataMember(Name = "TemperatureUnit")]
        private string TemperatureUnitName
        {
            get => Enum.GetName(typeof(TemperatureUnit), TemperatureUnit);
            set => TemperatureUnit = (TemperatureUnit) Enum.Parse(typeof(TemperatureUnit), value, true);
        }

        [DataMember]
        public double Humidity
        {
            get => Math.Round(_humidity, 2);
            private set => _humidity = value;
        }

        [DataMember]
        public double Temperature
        {
            get => Math.Round(_temperature, 2);
            private set => _temperature = value;
        }

        public TemperatureUnit TemperatureUnit { get; set; }

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

        private async void UpdateHumidity()
        {
            while (true)
                try
                {
                    await Task.Run(() =>
                    {
                        Humidity = GetHumidityValue();
                        var batteryLevel = GetBatteryLevel();
                        var measurement = new Measurement
                        {
                            SensorName = Name,
                            BatteryLevel = batteryLevel,
                            DateTime = DateTime.Now,
                            Value = Humidity,
                            Type = MeasurementType.Humidity
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

        private static double GetHumidityValue()
        {
            var random = new Random();
            return random.NextDouble() * 100;
        }

        private static double GetTemperatureValue()
        {
            var random = new Random();
            return random.NextDouble() * 60 - 30;
        }
    }
}