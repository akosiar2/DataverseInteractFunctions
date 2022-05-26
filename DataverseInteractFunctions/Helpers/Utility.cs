using System;
using System.Collections.Generic;
using System.Text;

namespace DataverseInteractFunctions.Helpers
{
    public static class Utility
    {
        public static IEnumerable<DateTime> AllDatesBetween(DateTime start, DateTime end)
        {
            for (var day = start.Date; day <= end; day = day.AddDays(1))
                yield return day;
        }
    }
}
