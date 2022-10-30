using System;

namespace VacationMachine
{
    public class Program
    {
        private static void Main()
        {
            var vacationDatabase = new VacationDatabase();
            var messageBus = new MessageBus();
            var emailSender = new EmailSender();
            var escalationManager = new EscalationManager();

            var vacationService = new VacationService(
                vacationDatabase,
                messageBus,
                emailSender,
                escalationManager,
                new Mapper(vacationDatabase,
                messageBus,
                emailSender,
                escalationManager)
            );

            var result = vacationService.RequestPaidDaysOff(3, 1);

            Console.WriteLine($"Vacation is: {result}");
            Console.ReadKey();
        }
    }
}
