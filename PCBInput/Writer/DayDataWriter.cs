using DBLib;
using DBLib.Record;
using DBLib.Record.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBInput.Writer
{
    public class DayDataWriter : IDataWriter<DayEndRecord>
    {
        private IUnitOfWork<IDayEndRecordRepository> work;

        public DayDataWriter(IUnitOfWork<IDayEndRecordRepository> work)
        {
            this.work = work;
        }

        public void Write(List<DayEndRecord> data, DateTime date)
        {
            var dbDate = work.Repo.GetDbContextDate();
            if (date.Hour != dbDate.Hour)
            {
                RenewUnitOfWork(date);
            }
            work.Repo.AddRange(data);
        }

        private void RenewUnitOfWork(DateTime date)
        {
            work.Dispose();
            work = (IUnitOfWork<IDayEndRecordRepository>)new UnitOfWork<DayEndRecordRepository>(new SendDataContext(date));
        }
    }
}
