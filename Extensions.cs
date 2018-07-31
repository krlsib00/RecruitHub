using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDataGenerator
{
    public static class Extensions
    {
        static Random rnd = new Random();

        public static T PickRandom<T>(this IList<T> values)
        {
            return values[rnd.Next(0, values.Count)];
        }
    }
}
