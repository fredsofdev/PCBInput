using Xunit;
using PCBInput.Helper;
using System;
using System.Collections.Generic;
using System.Threading;
using DBLib;
using DBLib.Record;
using Moq;
using DBLib.Setup;
using DBLib.Setup.Entities;

namespace PCBInput.Test
{
    public class TimeCallerTest
    {
        
        public void TimeCallerTimeInterval()
        {
            var eventLists = new List<DateTime>();

            var dbSetMock = Helper.getMockDbSet(Constants.SETTING);
            var context = new Mock<SettingContext>();


            context.Setup(x => x.Set<Setting>()).Returns(dbSetMock.Object);

            var timer = new TimeScheduleEvent(new UnitOfWork<SetupRepository<Setting>, SettingContext>(context.Object));

            timer.fiveSecondElapsed += delegate (object? source, TimeScheduleEventArgs e)
            {
                eventLists.Add(e.DateTime);
            };
            Thread.Sleep(10000);


            eventLists.ForEach(dateTime =>
            {
                Assert.True(
                    dateTime.Second % 5 == 0, 
                    $"Event call time must be 5 devidable but {dateTime.Second}");
            });
        }
    }
}