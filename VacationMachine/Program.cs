using System;
using System.Collections.Generic;
using VacationMachine.ResultHandler;
using VacationMachine.ResultHandler.Interfaces;

namespace VacationMachine
{
    public class Program
    {
        static void Main(string[] args)
        {
            VacationService service = new VacationService(new VacationDatabase(), new ResultCalculator(), GetResultHandlers());
            var result = service.RequestPaidDaysOff(3, 1);

            Console.WriteLine($"Vacation is :{result}");
            Console.ReadKey();
        }

        private static IEnumerable<IResultHandler> GetResultHandlers()
        {
            return new List<IResultHandler>()
            {
                new MessageBusResultHandler(new MessageBus()), new EmailSenderResultHandler(new EmailSender()), new EscalationManagerResultHandler(new EscalationManager()),
            };
        }
    }
}
