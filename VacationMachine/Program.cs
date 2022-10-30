using System;

namespace VacationMachine
{
    public class Program
    {
        private static void Main()
        {
            VacationService vacationService = CreateVacationService();

            var result = vacationService.RequestPaidDaysOff(3, 1);

            Console.WriteLine($"Vacation is: {result}");
            Console.ReadKey();
        }

        private static VacationService CreateVacationService()
        {
            var employeeManager = new EmployeeManager(
                new VacationDatabase(),
                new EmployeeMapper(
                    new MessageBus(),
                    new EmailSender(),
                    new EscalationManager()
                )
            );

            return new VacationService(employeeManager);
        }
    }
}
