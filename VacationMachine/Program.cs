using System;

namespace VacationMachine
{
    public class Program
    {
        static void Main(string[] args)
        {
            VacationService service = new VacationService(new VacationDatabase(), new MessageBus(), new EmailSender(), new EscalationManager(), new ResultCalculator());
            var result = service.RequestPaidDaysOff(3, 1);

            Console.WriteLine($"Vacation is :{result}");
            Console.ReadKey();
        }
    }
}
