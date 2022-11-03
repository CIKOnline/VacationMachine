using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationMachine.Services.Models;

namespace VacationMachine.Services.VacationStrategies
{
    public interface IVacationStrategy
    {
        Result ExecuteStrategy(int days);
    }
}