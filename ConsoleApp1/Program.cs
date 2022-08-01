

using DBLib.Record.Entities;
using Microsoft.Extensions.DependencyInjection;
using PCBInput;

namespace ConsoleApp1 // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
           var service = PCBService.GetPCBServices();

           var OffData = service.GetService<TimedTask<DateTime, IGrouping<DateTime, SendItem>>>();

            OffData.Execute(DateTime.Now);
        
        }
    }
}

