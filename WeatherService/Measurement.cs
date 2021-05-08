using System;
using System.Runtime.Serialization;

namespace WeatherService
{
    [DataContract]
    public class Measurement
    {
        [DataMember] public string SensorName { get; init; }

        [DataMember] public int BatteryLevel { get; init; }

        [DataMember] public DateTime DateTime { get; init; }

        [DataMember] public double Value { get; init; }

        [DataMember] public MeasurementType Type { get; init; }
    }
}