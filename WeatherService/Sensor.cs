using System;
using System.Runtime.Serialization;

namespace WeatherService
{
    [DataContract]
    public class Sensor
    {
        public delegate void MeasurementEventHandler(Measurement measurement);

        private static int _sensorCount = 1;
        private string _name;

        protected Sensor()
        {
            _name = "Sensor" + _sensorCount;
            _sensorCount++;
        }

        [DataMember]
        public string Name
        {
            get => _name;
            set
            {
                if (value.Length > 16) throw new ArgumentOutOfRangeException(value);
                _name = value;
            }
        }

        public event MeasurementEventHandler Measured;

        protected static int GetBatteryLevel()
        {
            var random = new Random();
            return random.Next(0, 101);
        }

        protected void MeasurementChanged(Measurement measurement)
        {
            Measured?.Invoke(measurement);
        }
    }
}