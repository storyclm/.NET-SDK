using System;

namespace StoryCLM.SDK
{
    public class DaySection : Section
    {
        internal int? _day;
        internal MonthSection _monthSection;

        public HourSection Day(int day)
        {
            _day = day;
            HourSection hourSection = new HourSection();
            hourSection._daySection = this;
            return hourSection;
        }

        public override string ToString()
        {
            if (!_day.HasValue) return _monthSection?.ToString();
            int month = _monthSection?._month != null ? _monthSection._month.Value : DateTime.UtcNow.Month;
            int year = _monthSection?._yearSection?._year != null ? _monthSection._yearSection._year.Value : DateTime.UtcNow.Year;
            if (!Validate(year, month, _day.Value, 0)) throw new InvalidOperationException("Invalid day section.");
            return $@"{year}/{month}/{_day.Value}/";
        }

    }
}
