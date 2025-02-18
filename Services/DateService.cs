namespace FinancialAssistent.Services
{
    public static class DateService
    {
        public static DateOnly GetStartOfWeek(DateTime date, DateTime firstDayOfMonth, DateTime lastDayOfMonth)
        {
            DayOfWeek dayOfWeek = date.DayOfWeek;

            int diff = dayOfWeek == DayOfWeek.Sunday ? 6 : (int)dayOfWeek - 1;
            DateOnly weekStart = DateOnly.FromDateTime(date.AddDays(-diff));

            if (weekStart < DateOnly.FromDateTime(firstDayOfMonth))
            {
                return DateOnly.FromDateTime(firstDayOfMonth);
            }

            return weekStart;
        }

        public static List<DateOnly> GetWeekStartDates(DateTime firstDayOfMonth, DateTime lastDayOfMonth)
        {
            List<DateOnly> weekStartDates = new List<DateOnly>();

            DateOnly current = DateOnly.FromDateTime(firstDayOfMonth);
            DateOnly lastDay = DateOnly.FromDateTime(lastDayOfMonth);

            DayOfWeek firstDayWeek = firstDayOfMonth.DayOfWeek;
            int daysToSunday = firstDayWeek == DayOfWeek.Sunday ? 0 : 7 - (int)firstDayWeek;
            DateOnly firstWeekEnd = current.AddDays(daysToSunday);
            weekStartDates.Add(current);

            while (firstWeekEnd < lastDay)
            {
                current = firstWeekEnd.AddDays(1);
                weekStartDates.Add(current);

                firstWeekEnd = current.AddDays(6);
                if (firstWeekEnd > lastDay)
                    firstWeekEnd = lastDay;
            }

            return weekStartDates;
        }
    }
}
