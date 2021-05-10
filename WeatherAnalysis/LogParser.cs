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
        private static readonly string BasePath =
            Path.GetFullPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "weatherservice"));

        public static List<Measurement> FindMeasurements(Func<Measurement, bool> predicate)
        {
            return GetAllMeasurements().Where(predicate).ToList();
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
            string text;
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var textReader = new StreamReader(fileStream))
            {
                text = textReader.ReadToEnd();
            }

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
            return !Directory.Exists(BasePath) ? Enumerable.Empty<string>() : Directory.GetFiles(BasePath, "*.json");
        }
    }
}