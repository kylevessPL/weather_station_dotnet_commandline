namespace WeatherService
{
    public interface ITemperature
    {
        double Temperature { get; }
        TemperatureUnit TemperatureUnit { get; set; }
    }
}