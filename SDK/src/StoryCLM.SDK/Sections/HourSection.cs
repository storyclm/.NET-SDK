using System;

namespace StoryCLM.SDK
{
    public class HourSection : Section
    {
        internal DaySection _daySection;

        internal int? _hour;

        public Section Hour(int hour)
        {
            _hour = hour;
            return this;
        }

        public override string ToString()
        {
            if (!_hour.HasValue) return _daySection?.ToString();
            int day = _daySection?._day != null ? _daySection._day.Value : DateTime.UtcNow.Day;
            int month = _daySection?._monthSection?._month != null ? _daySection._monthSection._month.Value : DateTime.UtcNow.Month;
            int year = _daySection?._monthSection?._yearSection?._year != null ? _daySection._monthSection._yearSection._year.Value : DateTime.UtcNow.Year;
            if (!Validate(year, month, day, _hour.Value)) throw new InvalidOperationException("Invalid hour section.");
            return $@"{year}/{month}/{day}/{_hour.Value}/";
        }
    }
}
