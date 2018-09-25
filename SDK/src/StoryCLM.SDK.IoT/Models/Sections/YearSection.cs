using System;
using System.Collections.Generic;
using System.Text;

namespace StoryCLM.SDK.IoT.Models
{
    public class YearSection : Section
    {
        internal int? _year;

        public MonthSection Year(int year)
        {
            _year = year;
            MonthSection monthSection = new MonthSection();
            monthSection._yearSection = this;
            return monthSection;
        }

        public override string ToString()
        {
            if (!_year.HasValue) return null;
            if (!Validate(_year.Value, 1, 1, 0)) throw new InvalidOperationException("Invalid year section.");
            return $@"{_year}/";
        }
    }
}
