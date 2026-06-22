using SkyRoute.Domain.Enums;

namespace SkyRoute.Application.Common.Helpers
{
    public static class DateHelper
    {
        public static OperatingDays ConvertDayToFlag(DayOfWeek day)
        {
            return day switch
            {
                DayOfWeek.Monday => OperatingDays.Monday,
                DayOfWeek.Tuesday => OperatingDays.Tuesday,
                DayOfWeek.Wednesday => OperatingDays.Wednesday,
                DayOfWeek.Thursday => OperatingDays.Thursday,
                DayOfWeek.Friday => OperatingDays.Friday,
                DayOfWeek.Saturday => OperatingDays.Saturday,
                DayOfWeek.Sunday => OperatingDays.Sunday,
                _ => OperatingDays.None
            };
        }
    }
}