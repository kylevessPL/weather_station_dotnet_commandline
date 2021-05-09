using System;
using System.Collections.Generic;
using System.Globalization;
using WeatherExtensions;
using WeatherService;

namespace WeatherAnalysis
{
    public static class Program
    {
        public static void Main()
        {
            for (;;)
            {
                PrintBasicCommandList();
                Console.WriteLine("Choose operation: ");
                var operationChoice = int.TryParse(Console.ReadLine(), out var outValue)
                    ? outValue
                    : default(int?);
                Predicate<Measurement> predicate;
                switch (operationChoice)
                {
                    case 1:
                        predicate = BuildFindBySensorNamePredicate();
                        break;
                    case 2:
                        predicate = BuildFindByTypeAndValueRangePredicate();
                        break;
                    case 3:
                        predicate = BuildFindByDatetimeRangePredicate();
                        break;
                    case 4:
                        predicate = BuildFindByBatteryLevelMaxPredicate();
                        break;
                    case 5:
                        predicate = BuildFindByCustomCriteriaPredicate();
                        break;
                    default:
                        Console.WriteLine("Invalid option");
                        continue;
                }

                var measurements = LogParser.FindMeasurements(predicate);
                string sortChoice;
                for (;;)
                {
                    Console.WriteLine("Sort measurements? y/n");
                    sortChoice = Console.ReadLine();
                    sortChoice = sortChoice?.Trim().ToLower();
                    if (!string.IsNullOrEmpty(sortChoice) && (sortChoice.Equals("y") || sortChoice.Equals("n"))) break;
                    Console.WriteLine("Invalid choice");
                }

                if (sortChoice.Equals("y"))
                {
                    var func = BuildSortFunc();
                    var sortDirection = GetSortDirection();
                    measurements = LogParser.SortMeasurements(measurements, func, sortDirection);
                }

                PrintMeasurements(measurements);
            }
        }

        private static Predicate<Measurement> BuildFindBySensorNamePredicate()
        {
            for (;;)
            {
                Console.WriteLine("Type sensor name: ");
                var name = Console.ReadLine();
                if (!string.IsNullOrEmpty(name)) return measurement => measurement.SensorName.Equals(name.Trim());
                Console.WriteLine("Sensor name cannot be empty");
            }
        }

        private static Predicate<Measurement> BuildFindByTypeAndValueRangePredicate()
        {
            PrintMeasurementTypeList();
            int choice;
            for (;;)
            {
                Console.WriteLine("Choose measurement type: ");
                choice = int.TryParse(Console.ReadLine(), out var outValue)
                    ? outValue
                    : default;
                if (choice.InRange(1, 3)) break;
                Console.WriteLine("Invalid option");
            }

            var measurementType = (MeasurementType) Enum.ToObject(typeof(MeasurementType), choice - 1);
            double minValue;
            for (;;)
            {
                Console.WriteLine("Type minimum measurement value: ");
                if (double.TryParse(Console.ReadLine(), out minValue)) break;
                Console.WriteLine("Not a valid number");
            }

            double maxValue;
            for (;;)
            {
                Console.WriteLine("Type maximum measurement value: ");
                if (double.TryParse(Console.ReadLine(), out maxValue)) break;
                Console.WriteLine("Not a valid number");
            }

            return measurement =>
                measurement.Type.Equals(measurementType) &&
                measurement.Value.InRange(minValue, maxValue);
        }

        private static Predicate<Measurement> BuildFindByDatetimeRangePredicate()
        {
            string[] formats = {"dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy"};
            DateTime minValue;
            for (;;)
            {
                Console.WriteLine("Type minimum datetime value (dd/MM/yyyy HH:mm:ss or dd/MM/yyyy format): ");
                if (DateTime.TryParseExact(Console.ReadLine(),
                    formats, CultureInfo.CurrentCulture, DateTimeStyles.None,
                    out minValue))
                    break;
                Console.WriteLine("Not a valid datetime");
            }

            DateTime maxValue;
            for (;;)
            {
                Console.WriteLine("Type maximum datetime value (dd/MM/yyyy HH:mm:ss or dd/MM/yyyy format): ");
                if (DateTime.TryParse(Console.ReadLine(), out maxValue)) break;
                Console.WriteLine("Not a valid datetime");
            }

            return measurement => measurement.DateTime.InRange(minValue, maxValue);
        }

        private static Predicate<Measurement> BuildFindByBatteryLevelMaxPredicate()
        {
            int value;
            for (;;)
            {
                Console.WriteLine("Type maximum battery level: ");
                if (int.TryParse(Console.ReadLine(), out value) && value.InRange(0, 100)) break;
                Console.WriteLine("Not a valid battery level");
            }

            return measurement => measurement.BatteryLevel <= value;
        }

        private static Predicate<Measurement> BuildFindByCustomCriteriaPredicate()
        {
            Predicate<Measurement> predicate = _ => true;
            for (;;)
            {
                PrintCustomCriteriaCommandList();
                Console.WriteLine("Choose operation: ");
                var choice = int.TryParse(Console.ReadLine(), out var outValue)
                    ? outValue
                    : default(int?);
                Predicate<Measurement> value;
                switch (choice)
                {
                    case 1:
                        value = BuildFindBySensorNamePredicate();
                        break;
                    case 2:
                        value = BuildFindByTypeAndValueRangePredicate();
                        break;
                    case 3:
                        value = BuildFindByDatetimeRangePredicate();
                        break;
                    case 4:
                        value = BuildFindByBatteryLevelMaxPredicate();
                        break;
                    case 5:
                        return predicate;
                    default:
                        Console.WriteLine("Invalid option");
                        continue;
                }

                predicate = predicate.And(value);
            }
        }

        private static Func<Measurement, object> BuildSortFunc()
        {
            PrintMeasurementPropertyList();
            for (;;)
            {
                Console.WriteLine("Choose sort property: ");
                var propertyChoice = int.TryParse(Console.ReadLine(), out var outValue)
                    ? outValue
                    : default(int?);
                switch (propertyChoice)
                {
                    case 1:
                        return measurement => measurement.SensorName;
                    case 2:
                        return measurement => measurement.BatteryLevel;
                    case 3:
                        return measurement => measurement.DateTime;
                    case 4:
                        return measurement => measurement.Value;
                    case 5:
                        return measurement => measurement.Type;
                    default:
                        Console.WriteLine("Invalid option");
                        continue;
                }
            }
        }

        private static SortDirection GetSortDirection()
        {
            PrintSortDirectionList();
            int directionChoice;
            for (;;)
            {
                Console.WriteLine("Choose sort direction: ");
                directionChoice = int.TryParse(Console.ReadLine(), out var outValue)
                    ? outValue
                    : default;
                if (directionChoice.InRange(1, 2)) break;
                Console.WriteLine("Invalid option");
            }

            return (SortDirection) Enum.ToObject(typeof(SortDirection), directionChoice - 1);
        }

        private static void PrintSortDirectionList()
        {
            Console.WriteLine("(1) Ascending");
            Console.WriteLine("(2) Descending");
        }

        private static void PrintMeasurementTypeList()
        {
            Console.WriteLine("(1) Temperature");
            Console.WriteLine("(2) Humidity");
            Console.WriteLine("(3) Pressure");
        }

        private static void PrintMeasurementPropertyList()
        {
            Console.WriteLine("(1) Sensor name");
            Console.WriteLine("(2) Battery level");
            Console.WriteLine("(3) Datetime");
            Console.WriteLine("(4) Value");
            Console.WriteLine("(5) Type");
        }

        private static void PrintBasicCommandList()
        {
            Console.WriteLine("(1) Find all measurements by sensor name");
            Console.WriteLine("(2) Find all measurements by measurement type and value in range");
            Console.WriteLine("(3) Find all measurements by datetime in range");
            Console.WriteLine("(4) Find all measurements by maximum battery level");
            Console.WriteLine("(5) Find all measurements meeting custom criteria");
        }

        private static void PrintCustomCriteriaCommandList()
        {
            Console.WriteLine("(1) Find all measurements by sensor name");
            Console.WriteLine("(2) Find all measurements by measurement type and value in range");
            Console.WriteLine("(3) Find all measurements by datetime in range");
            Console.WriteLine("(4) Find all measurements by maximum battery level");
            Console.WriteLine("(5) End building criteria");
        }

        private static void PrintMeasurements(List<Measurement> measurements)
        {
            Console.WriteLine();
            measurements.ForEach(measurement =>
            {
                Console.WriteLine("<== Sensor data begin ==>");
                Console.WriteLine("Sensor name: " + measurement.SensorName);
                Console.WriteLine("Sensor battery level: " + measurement.BatteryLevel);
                Console.WriteLine("Measurement datetime: " + measurement.DateTime);
                Console.WriteLine("Measurement value: " + measurement.Value);
                Console.WriteLine("Measurement type: " + measurement.Type);
                Console.WriteLine("<== Sensor data end ==>");
            });
            Console.WriteLine();
        }
    }
}