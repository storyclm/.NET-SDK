using System;
using System.Collections.Generic;
using System.Text;

namespace SroryCLM.SDK.CLMAnalitycs
{
    public static class UtilsExtensions
    {
        public static long ToUnix(this DateTime dt) =>
            (long)(dt - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

        public static string ToQuery(this DateTime? date, string name, bool last = true) =>
            date == null ? null : $"{name}={date.Value.ToUnix()}{(last ? string.Empty : "&")}";
    }
}
