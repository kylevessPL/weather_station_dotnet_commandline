using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using WeatherService;

namespace WeatherAnalysis
{
    public static class LogParser
    {
        private static readonly string BasePath = AppContext.BaseDirectory;

        public static List<Measurement> FindMeasurements(Predicate<Measurement> predicate)
        {
            return GetAllMeasurements().ToList().FindAll(predicate);
        }

        public static List<Measurement> SortMeasurements<TKey>(
            IEnumerable<Measurement> measurements,
            Func<Measurement, TKey> sort,
            SortDirection direction)
        {
            return direction == SortDirection.Ascending
                ? measurements.OrderBy(sort).ToList()
                : measurements.OrderByDescending(sort).ToList();
        }

        private static IEnumerable<Measurement> GetAllMeasurements()
        {
            var measurementList = Enumerable.Empty<Measurement>().ToList();
            GetAllJsonFiles().ToList()
                .ForEach(file => measurementList.AddRange(ReadJsonFile(file)));
            return measurementList;
        }

        private static IEnumerable<Measurement> ReadJsonFile(string path)
        {
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

        private static IEnumerable<string> GetAllJsonFiles()
        {
            return Directory.GetFiles(BasePath, "*.json");
        }
    }
}