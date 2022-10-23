using System;
using VacationMachine.Database;
using VacationMachine.Email;
using VacationMachine.Escalation;

namespace VacationMachine;

public class Program
{
    private static void Main(string[] args)
    {
        var service = new VacationService(new VacationDatabase(), new MessageBus.MessageBus(), new EmailSender(),
            new EscalationManager());
        Result result = service.RequestPaidDaysOff(3, 1);

        Console.WriteLine($"Vacation is :{result}");
        Console.ReadKey();
    }
}