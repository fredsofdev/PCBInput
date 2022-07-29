
using DBLib;
using DBLib.Setup;
using DBLib.Setup.Entities;
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
        public event EventHandler<TimeScheduleEventArgs>? unsendTimeElapsed;

        private IUnitOfWork<SetupRepository<Setting>> workOfSetting;
        public TimeScheduleEvent(IUnitOfWork<SetupRepository<Setting>> unitOfWork)
        {
            this.workOfSetting = unitOfWork;
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
                args.DateTime = e.SignalTime.AddDays(-1);
                dayElapsed?.Invoke(this, args);
            }

            var setting = workOfSetting.Repo.GetFirst();
            if(setting.UnsendTime == e.SignalTime.ToString("HHmm") || 
                (setting.UnsendTime == "9999" && e.SignalTime == DateTime.Today))
            {
                unsendTimeElapsed?.Invoke(this, args);
            }

        }

    }

    public class TimeScheduleEventArgs : EventArgs
    {
        public DateTime DateTime { get; set; }
    }
}
