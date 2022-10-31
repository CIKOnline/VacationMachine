using System;

namespace VacationMachine
{
    public class Configuration
    {
        public static int GetMaxDaysForPerformers()
        {
            return 45;
        }

        public static int GetMaxDays()
        {
            return 26;
        }

        public static string GetEmailText()
        {
            return "next time";
        }

        public static string GetEventText()
        {
            return "request approved";
        }
    }
}
