using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiVacunacion.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime ToMexicoTime(this DateTime d)
        {
            return TimeZoneInfo.ConvertTime(d,
               TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time (Mexico)"));


        }
    }
}
