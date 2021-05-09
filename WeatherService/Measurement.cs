using System;
using System.Text.Json.Serialization;

namespace WeatherService
{
    public class Measurement
    {
        [JsonPropertyName("sensor_name")] public string SensorName { get; init; }

        [JsonPropertyName("battery_level")] public int BatteryLevel { get; init; }

        [JsonPropertyName("datetime")] public DateTime DateTime { get; init; }

        [JsonPropertyName("value")] public double Value { get; init; }

        [JsonIgnore] public MeasurementType Type { get; init; }

        [JsonPropertyName("type")]
        public string MeasurementTypeName
        {
            get => Enum.GetName(typeof(MeasurementType), Type);
            init => Type = (MeasurementType) Enum.Parse(typeof(MeasurementType), value, true);
        }
    }
}