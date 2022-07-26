
using System;
using System.Timers;

namespace PCBInput.Helper
{
    public class TimeScheduleEvent
    {
        private System.Timers.Timer _timer;
        public event EventHandler<TimeScheduleEventArgs>? fiveSecondElapsed;
        public event EventHandler<TimeScheduleEventArgs>? fiveMinuteElapsed;
        public event EventHandler<TimeScheduleEventArgs>? dayElapsed;

        public TimeScheduleEvent()
        {
            _timer = new System.Timers.Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += OnElapsedTime;
            _timer.Enabled = true;
        }

        private void OnElapsedTime(object? source, ElapsedEventArgs e)
        {
            TimeScheduleEventArgs args = new TimeScheduleEventArgs();
            args.DateTime = e.SignalTime;
            if (e.SignalTime.Second % 5 == 0)
            {
                fiveSecondElapsed?.Invoke(this, args);
            }

            if (e.SignalTime.Minute % 5 == 0 && e.SignalTime.Second == 0)
            {
                fiveMinuteElapsed?.Invoke(this, args);
            }

            if (e.SignalTime == DateTime.Today)
            {
                dayElapsed?.Invoke(this, args);
            }
        }

    }

    public class TimeScheduleEventArgs : EventArgs
    {
        public DateTime DateTime { get; set; }
    }
}
