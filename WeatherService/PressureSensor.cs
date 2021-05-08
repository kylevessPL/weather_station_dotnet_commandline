using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WeatherService
{
    [DataContract]
    public class PressureSensor : Sensor, IPressure
    {
        private const int Interval = 5;
        private double _pressure;

        public PressureSensor()
        {
            UpdatePressure();
        }

        [DataMember]
        public double Pressure
        {
            get => Math.Round(_pressure, 2);
            private set => _pressure = value;
        }

        private async void UpdatePressure()
        {
            while (true)
                try
                {
                    await Task.Run(() =>
                    {
                        Pressure = GetPressureValue();
                        var batteryLevel = GetBatteryLevel();
                        var measurement = new Measurement
                        {
                            SensorName = Name,
                            BatteryLevel = batteryLevel,
                            DateTime = DateTime.Now,
                            Value = Pressure,
                            Type = MeasurementType.Pressure
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

        private static double GetPressureValue()
        {
            var random = new Random();
            return random.NextDouble() * 40 + 980;
        }
    }
}