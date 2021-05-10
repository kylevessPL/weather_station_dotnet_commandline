using System;

namespace WeatherExtensions
{
    public static class PredicateExtensions
    {
        public static Func<T, bool> And<T>(this Func<T, bool> p1, Func<T, bool> p2)
        {
            return x => p1(x) && p2(x);
        }
    }
}