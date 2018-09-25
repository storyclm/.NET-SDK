using System;
using System.Collections.Generic;
using System.Text;

namespace StoryCLM.SDK.IoT.Models
{
    public class MonthSection : Section
    {
        internal int? _month;
        internal YearSection _yearSection;

        public DaySection Month(int month)
        {
            _month = month;
            DaySection daySection = new DaySection();
            daySection._monthSection = this;
            return daySection;
        }

        public override string ToString()
        {
            if (!_month.HasValue) return _yearSection?.ToString();
            int year = _yearSection?._year != null ? _yearSection._year.Value : DateTime.UtcNow.Year;
            if (!Validate(year, _month.Value, 1, 0)) throw new InvalidOperationException("Invalid month section.");
            return $@"{year}/{_month.Value}/";
        }
    }
}
