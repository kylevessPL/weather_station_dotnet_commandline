using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace WeatherService
{
    public class WeatherStation
    {
        private static readonly string BasePath = AppContext.BaseDirectory;

        private static int _stationCount = 1;

        private readonly ReaderWriterLockSlim _lock;
        private readonly string _name;

        public WeatherStation()
        {
            _lock = new ReaderWriterLockSlim();
            _name = "Station" + _stationCount;
            _stationCount++;
        }

        public void RegisterSensor(Sensor sensor)
        {
            sensor.Measured += measurement =>
            {
                PrintMeasurement(measurement);
                WriteDataToJson(measurement);
            };
        }

        private static void PrintMeasurement(Measurement measurement)
        {
            Console.WriteLine("<== Sensor update begin ==>");
            Console.WriteLine("Sensor name: " + measurement.SensorName);
            Console.WriteLine("Sensor battery level: " + measurement.BatteryLevel);
            Console.WriteLine("Measurement datetime: " + measurement.DateTime);
            Console.WriteLine("Measurement value: " + measurement.Value);
            Console.WriteLine("Measurement type: " + measurement.Type);
            Console.WriteLine("<== Sensor update end ==>");
        }

        private void WriteDataToJson(Measurement measurement)
        {
            var path = Path.Combine(BasePath, Path.GetFileName(_name) + ".json");
            _lock.EnterWriteLock();
            try
            {
                var measurementList = ReadJsonFile(path).ToList();
                measurementList.Add(measurement);
                var jsonData = SerializeJson(measurementList);
                File.WriteAllText(path, jsonData);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        private static IEnumerable<Measurement> ReadJsonFile(string path)
        {
            if (!File.Exists(path)) return Enumerable.Empty<Measurement>();
            var text = File.ReadAllText(path);
            try
            {
                return (List<Measurement>) JsonSerializer.Deserialize(text, typeof(List<Measurement>));
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Error deserializing json file");
                throw;
            }
        }

        private static string SerializeJson(List<Measurement> measurementList)
        {
            return JsonSerializer.Serialize(measurementList);
        }
    }
}