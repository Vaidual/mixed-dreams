using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Constants
{
    public class StatisticIntervals
    {
        public const string d1 = "d1";
        public const string d7 = "d7";
        public const string m1 = "m1";

        public static IReadOnlyDictionary<string, TimeSpan> IntervalTimespan = new Dictionary<string, TimeSpan>
        {
            {d1, TimeSpan.FromDays(1)},
            {d7, TimeSpan.FromDays(7)},
            {m1, TimeSpan.FromDays(30)},
        };
    }
}
