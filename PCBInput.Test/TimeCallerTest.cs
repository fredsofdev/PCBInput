using Xunit;
using PCBInput.Helper;
using System;
using System.Collections.Generic;
using System.Threading;

namespace PCBInput.Test
{
    public class TimeCallerTest
    {
        
        public void TimeCallerTimeInterval()
        {
            var eventLists = new List<DateTime>();
            var timer = new TimeScheduleEvent();

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