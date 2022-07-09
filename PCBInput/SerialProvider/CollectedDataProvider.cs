using DBLib;
using DBLib.Record;
using DBLib.Record.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBInput.SerialProvider
{
    public class DailyDataProvider : IDataProvider<DayEndRecord>
    {
        private IUnitOfWork<IDayEndRecordRepository> work;

        public DailyDataProvider(IUnitOfWork<IDayEndRecordRepository> work)
        {
            this.work = work;
        }

        public List<DayEndRecord> GetData(DateTime date)
        {
            var dbDate = work.Repo.GetDbContextDate();
            if (date.Hour != dbDate.Hour)
            {
                RenewUnitOfWork(date);
            }
            return work.Repo.GetAll().ToList();
        }

        private void RenewUnitOfWork(DateTime date)
        {
            work.Dispose();
            work = (IUnitOfWork<IDayEndRecordRepository>)new UnitOfWork<DayEndRecordRepository>(new RecordDataContext(date));
        }
    }
}
