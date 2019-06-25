using System;

namespace FirmaBudowlana.Infrastructure.Extensions
{
    public static class DaysWithoutWeekends
    {
        public static int Count(DateTime start, DateTime end)
        {
            var days = end.DayOfYear - start.DayOfYear;

            var startDate = start;

            if (days == 0) return 1;
            else if (days < 0) throw new Exception("End date cannot be previous start date");

            while (startDate.Date != end.Date)
            {
                if (startDate.DayOfWeek == DayOfWeek.Saturday || startDate.DayOfWeek == DayOfWeek.Sunday) days--;
                startDate = startDate.AddDays(1);
            }

            return days;
        }
    }
}
