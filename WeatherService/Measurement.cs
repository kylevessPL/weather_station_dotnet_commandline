using System;
using System.Runtime.Serialization;

namespace WeatherService
{
    [DataContract]
    public class Measurement
    {
        [DataMember] public string SensorName { get; set; }

        [DataMember] public int BatteryLevel { get; set; }

        [DataMember] public DateTime DateTime { get; set; }

        [DataMember] public double Value { get; set; }

        public MeasurementType Type { get; set; }

        [DataMember(Name = "Type")]
        private string MeasurementTypeName
        {
            get => Enum.GetName(typeof(MeasurementType), Type);
            set => Type = (MeasurementType) Enum.Parse(typeof(MeasurementType), value, true);
        }
    }
}