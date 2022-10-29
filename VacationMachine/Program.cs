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
            VacationService service = GetVacationService();
            var result = service.RequestPaidDaysOff(3, 1);

            Console.WriteLine($"Vacation is :{result}");
            Console.ReadKey();
        }

        private static VacationService GetVacationService()
        {
            var database = new VacationDatabase();

            return new VacationService(database, new ResultCalculator(), GetResultHandlers(database));
        }

        private static IEnumerable<IResultHandler> GetResultHandlers(VacationDatabase database)
        {
            return new List<IResultHandler>()
            {
                new VacationDatabaseResultHandler(database),
                new MessageBusResultHandler(new MessageBus()), 
                new EmailSenderResultHandler(new EmailSender()), 
                new EscalationManagerResultHandler(new EscalationManager()),
            };
        }
    }
}
