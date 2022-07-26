using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBLib;
using DBLib.Record.Entities;
using Microsoft.Extensions.DependencyInjection;
using PCBInput.DataProvider;
using PCBInput.Manipulator;
using PCBInput.Writer;

namespace PCBInput
{
    public static class ProtoService
    {
        public static IServiceProvider GetProtoServices()
        {
            var service = DBService.GetDBServices();

            service.AddTransient<IDataProvider<Item>, CollectedDataProvider>();
            service.AddTransient<IDataProvider<SendItem>, DailyDataProvider>();
            service.AddTransient<IDataProvider<int>, SerialModbusProvider>();
            service.AddTransient<IDataProvider<DateTime>, OffDataProvider>();

            service.AddTransient<IManipulator<Item, SendItem>, CollectionMerge>();
            service.AddTransient<IManipulator<SendItem, DayEndRecord>, DailyMerge>();
            service.AddTransient<IManipulator<int, Item>, MeasureSensor>();
            service.AddTransient<IManipulator<DateTime, IGrouping<DateTime, SendItem>>, OffDataConverter>();

            service.AddTransient<IDataWriter<DayEndRecord>, DayDataWriter>();
            service.AddTransient<IDataWriter<SendItem>, MinuteDataWriter>();
            service.AddTransient<IDataWriter<Item>, SecondDataWriter>();
            service.AddTransient<IDataWriter<IGrouping<DateTime, SendItem>>, OffDataWriter>();

            service.AddTransient<TimedTask<int, Item>>();
            service.AddTransient<TimedTask<Item, SendItem>>();
            service.AddTransient<TimedTask<SendItem, DayEndRecord>>();
            service.AddTransient<TimedTask<DateTime, IGrouping<DateTime, SendItem>>>();

            return service.BuildServiceProvider();
        }
    }
}
