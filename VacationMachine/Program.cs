using System;
using VacationMachine.Repository.Database;
using VacationMachine.Services;
using VacationMachine.Services.Email;
using VacationMachine.Services.Escalation;
using VacationMachine.Services.MessageBus;

namespace VacationMachine
{
    public class Program
    {
        private static void Main(string[] args)
        {
            VacationService service = new VacationService(new VacationDatabase(), new MessageBus(), new EmailSender(), new EscalationManager());
            var result = service.RequestPaidDaysOff(3, 1);

            Console.WriteLine($"Vacation is :{result}");
            Console.ReadKey();
        }
    }
}