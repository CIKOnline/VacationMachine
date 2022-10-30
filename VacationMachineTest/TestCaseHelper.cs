using System.Collections.Generic;

namespace VacationMachineTest
{
    public static class TestCaseHelper
    {
        public static IEnumerable<int> GetDaysFromTo(int firstDay, int lastDay)
        {
            for (var days = firstDay; days <= lastDay; days++)
            {
                yield return days;
            }
        }
    }
}
