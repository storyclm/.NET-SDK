using System;

namespace StoryCLM.SDK
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
