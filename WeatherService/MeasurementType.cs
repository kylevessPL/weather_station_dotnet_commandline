using System.Runtime.Serialization;

namespace WeatherService
{
    [DataContract]
    public enum MeasurementType
    {
        [EnumMember] Temperature,
        [EnumMember] Humidity,
        [EnumMember] Pressure
    }
}