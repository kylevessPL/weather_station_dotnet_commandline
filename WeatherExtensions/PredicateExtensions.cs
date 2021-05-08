using System;

namespace WeatherExtensions
{
    public static class PredicateExtensions
    {
        public static Predicate<T> And<T>(this Predicate<T> p1, Predicate<T> p2)
        {
            return x => p1(x) && p2(x);
        }
    }
}