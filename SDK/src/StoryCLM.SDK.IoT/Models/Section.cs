using System;
using System.Collections.Generic;
using System.Text;

namespace StoryCLM.SDK.IoT.Models
{
    public abstract class Section
    {
        internal bool Validate(int year, int month, int day, int hour)
        {
            try
            {
                new DateTime(year, month, day, hour, 0, 0);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
