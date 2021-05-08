namespace WeatherExtensions
{
    public static class NumberExtensions
    {
        public static bool InRange(this double num, double numMin, double numMax)
        {
            return num >= numMin && num <= numMax;
        }

        public static bool InRange(this int num, int numMin, int numMax)
        {
            return num >= numMin && num <= numMax;
        }
    }
}