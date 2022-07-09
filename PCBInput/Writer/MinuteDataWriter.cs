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
    public class MinuteDataWriter : IDataWriter<SendItem>
    {
        private IUnitOfWork<ISendRecordRepository> work;

        public MinuteDataWriter(IUnitOfWork<ISendRecordRepository> work)
        {
            this.work = work;
        }

        public void Write(List<SendItem> data, DateTime date)
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
            work = (IUnitOfWork<ISendRecordRepository>)new UnitOfWork<SendRecordRepository>(new SendDataContext(date));
        }
    }
}
